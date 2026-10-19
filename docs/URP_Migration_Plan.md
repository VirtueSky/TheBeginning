# Kiểm tra project và kế hoạch chuyển sang URP

Ngày kiểm tra: 19/10/2026. Project: TheBeginning.

> Đây là khảo sát trước migration. Triển khai và kết quả kiểm tra hiện tại ở [URP_Migration_Result.md](URP_Migration_Result.md).

## 1. Kết luận

Có thể chuyển project từ Built-in sang URP. Khối lượng chính nằm ở camera UI chạy qua nhiều scene additive, shader/material UI và build nội dung Addressables; chưa thấy nhu cầu viết lại gameplay để chuyển pipeline.

Đề xuất dùng **Universal Renderer, Forward** làm cấu hình thử nghiệm đầu tiên. Không chọn 2D Renderer chỉ vì camera orthographic: nội dung hiện tại chủ yếu là UGUI, TMP và UI particles; chưa có bằng chứng cần 2D Lighting. Chỉ chốt renderer sau khi thử đầy đủ luồng scene và VFX.

Đây là báo cáo khảo sát và kế hoạch, chưa cài URP hay chuyển asset. Không cam kết tăng FPS: cần đo trên thiết bị sau khi đạt tương đương hình ảnh.

## 2. Phạm vi và bằng chứng

Đã kiểm tra cấu hình Packages/ProjectSettings; thống kê Assets; đọc các luồng C# scene, level, popup, VFX, editor bootstrap và UI particles; tìm các API phụ thuộc render pipeline trong C# và shader. Kiểm tra chọn lọc package cache liên quan UIEffect, UIParticle, TMP, Spine và Sunflower.

Đã sử dụng **Coplay MCP** trực tiếp với Unity Editor:

- `set_unity_project_root`: xác nhận đúng thư mục project.
- `get_unity_editor_state`: Edit Mode, scene Launcher, không có compilation errors.
- `check_compile_errors`: trả về `No compile errors`.
- `execute_script`: chạy C# khảo sát chỉ đọc từ `/tmp/UrpReadOnlyAudit.cs`, lấy pipeline, target, color space, scene, camera, canvas, material và Shader.Find.

Không chạy Play Mode, build Android/iOS hay mở thay các scene đang làm việc. Scene Launcher đang mở có `isDirty = false`. Kết quả shader supported là trên Built-in hiện tại, không chứng minh tương thích URP. Đây không phải review logic từng dòng của toàn bộ dependency.

| Hạng mục | Kết quả |
|---|---|
| Unity trên đĩa và Editor | 6000.0.68f1 |
| Active build target | Android |
| Pipeline đang chạy | Built-in |
| Graphics pipeline asset | `m_CustomRenderPipeline: {fileID: 0}` |
| Quality | Một mức `Default`, không có pipeline override |
| Color space | Gamma |
| Quality hiện tại | MSAA 2x, shadow distance 20, pixel light count 1 |
| URP package | Chưa có trong manifest |
| Assets trên đĩa | 89 file C#, 16 shader, 6 `.mat`, 29 prefab, 4 scene |
| Scene thực tế | Entry, Launcher, Service, Game |
| Material qua AssetDatabase | 14 kết quả asset chứa material, bao gồm font/material nhúng; không phải 14 file `.mat` |
| Compile baseline | Coplay không báo lỗi compile |

Package đáng chú ý: UIEffect 5.9.6, ParticleEffectForUGUI 4.11.2, Spine runtime nhánh 4.1, Addressables 2.8.1, UGUI 2.0.0, Sunflower 3.5.8. Chưa có package Spine URP riêng trong manifest. Không suy ra Spine đang được dùng trong gameplay chỉ từ việc package đã cài.

## 3. Các phát hiện cần xử lý

### P0 — Build Settings trỏ scene không tồn tại

`ProjectSettings/EditorBuildSettings.asset` chỉ bật `Assets/_Project/Scenes/Initialization.unity`; file đó không có trong bốn scene thực tế. Play trực tiếp Launcher có thể che khuất vấn đề bootstrap khi build player.

`Assets/_Project/Scripts/EntryGame.cs` tải Launcher bằng Addressables. `LoadingManager.cs` tải Service additive rồi phát sự kiện tải Game. Cấu trúc gợi ý Entry là scene khởi động player, nhưng cần xác nhận component và tham chiếu của Entry trước khi đổi Build Settings. Kiểm tra Build Profiles đang có hiệu lực trong Editor, bảo đảm không có scene override khác.

Điều kiện trước migration: tạo được build Built-in chạy từ cold start với luồng scene đúng. Không dùng lỗi bootstrap sẵn có để đánh giá URP.

### P1 — Camera UI cần thiết kế lại rõ ràng

| Scene | Camera trên YAML | Canvas |
|---|---|---|
| Entry | Không có camera trực tiếp | Không có Canvas trực tiếp |
| Launcher | Orthographic, depth 10, SolidColor | Một Screen Space Camera |
| Game | Orthographic, depth 2, Depth-only | Một Screen Space Camera |
| Service | Orthographic, depth 3, Depth-only | Một Screen Space Camera và một Screen Space Overlay |

Coplay xác nhận Launcher đang có `Camera_Loading`, HDR/MSAA bật và `Canvas_Loading` gắn camera đó. Các scene còn lại được kiểm tra tĩnh trên YAML; chưa xác nhận camera bổ sung sinh từ prefab/runtime.

Không sao chép nguyên camera depth/clear flags rồi giả định tương đương URP. Prototype một Base camera và các Overlay camera cần thiết trên cùng Universal Renderer. Với Service tồn tại lâu hơn Game, phải xác định camera nào sở hữu stack theo vòng đời thực tế. Canvas Screen Space Overlay không nằm trong camera stack.

Nếu stack tham chiếu camera từ scene additive, bổ sung đăng ký khi load và gỡ khi unload; tham chiếu serialize giữa các scene độc lập không đủ. Cần thử Launcher → Service + Game, thay Game và quay lại, bảo đảm không clear mất UI, render hai lần hoặc giữ camera đã hủy.

### P1 — Shader dự phòng UI particle không tìm thấy ngay ở baseline

`Assets/ThirdParty/Extensions/UIParticle/UIParticleSystem.cs:79` gọi `Shader.Find("UI Extensions/Particles/Additive")` khi chưa gán material. Coplay trả về **missing**. Shader cục bộ `hidden particle.shader` khai báo tên khác: `UI/Particles/Hidden`, là pass discard, không phải shader additive thay thế.

Đây là rủi ro có sẵn, chỉ phát sinh ở nhánh fallback khi component không được gán material hợp lệ. Kiểm kê prefab nào còn dùng component legacy này; ưu tiên material tham chiếu trực tiếp, hỗ trợ UI stencil/clip/blend, hoặc chuyển có kiểm chứng sang package UIParticle hiện có. Kiểm tra stripping trên player, không chỉ Shader.Find trong Editor.

### P1 — Không chuyển hàng loạt shader UI sang URP/Lit

AssetDatabase ghi nhận ba material VFX dùng `UI/Additive`:

- `Assets/_Project/Fx/Fx_Star/Materials/Fx/fx_star_s_clear_white.mat`
- `Assets/_Project/Prefabs/VFX/Effect/Effect Rank/Materials/meteor_glow.mat`
- `Assets/_Project/Prefabs/VFX/Effect/Effect Rank/Materials/meteor_light_tray.mat`

Cùng với `UI/Hidden/UI-Effect-Shiny`, TMP Distance Field và TMP Mobile Distance Field, các shader này đang supported trên Built-in. Giữ nhóm UI riêng, kiểm tra clipping, stencil, soft mask, alpha/additive và sorting trên URP. `CGPROGRAM`/`UnityCG.cginc` tự nó không chứng minh shader UI không chạy được trên URP.

Hai shader TMP Surface có `#pragma surface` là đường render Built-in cần thay nếu có nội dung sử dụng. Chưa thấy material chính từ khảo sát dùng chúng; cần quét cả material subasset và dependency trước khi kết luận không dùng. Không thay shader TMP bằng URP/Lit vì sẽ mất logic SDF.

### P1 — Addressables là một phần bắt buộc của migration

Settings thực tế ở `Assets/ThirdParty/AddressableAssetsData/AddressableAssetSettings.asset`. Thư mục `Assets/AddressableAssetsData` cũng tồn tại nhưng không chứa bộ group chính.

Các group gồm Launcher, Service, Game, Levels, Default Local Group, Built In Data và Duplicate Asset Isolation. Game chứa nhiều popup; Levels có key `Level 1`, `Level 2`, `Level 3`. `EntryGame`, `LoadingManager`, `SceneLoader`, `LevelLoader` và `PopupManager` tải nội dung qua Addressables.

Remote catalog hiện tắt; profile vẫn có RemoteLoadPath `http://localhost/[BuildTarget]`. Cần xác định profile và schema path thực sự của từng group trước build, không mặc định mọi group đều remote hoặc đều local.

Sau chuyển pipeline phải clean build nội dung cho từng platform, sau đó build player tương ứng. Không trộn bundle Built-in cũ với player URP; không dùng riêng Asset Database Play Mode làm bằng chứng nghiệm thu. Nếu sau này phân phối remote, tách phiên bản catalog/bundle và giữ bộ tương ứng để rollback.

## 4. Nhận xét C#

Không tìm thấy `OnRenderImage`, custom CommandBuffer, replacement shader hay render pass tự viết trong C# thuộc Assets qua tìm kiếm hiện tại. Gameplay, currency, dịch vụ và VFX spawning phần lớn độc lập pipeline. `VfxSpawner.cs` spawn/despawn prefab qua pool; phần cần chuyển là renderer/material trong prefab.

UIEffect package có cả callback Built-in lẫn `RenderPipelineManager`, là dấu hiệu đã có nhánh SRP nhưng vẫn phải xác minh hình ảnh và build. Spine có các đường tạo material mặc định bằng `Spine/Skeleton`/`Spine/SkeletonGraphic`; nếu có SkeletonRenderer world-space, cần bộ shader URP tương thích runtime 4.1. SkeletonGraphic là UI, cần đánh giá riêng, không áp dụng thay shader hàng loạt.

Các điểm C# nên ghi nhận khi kiểm thử baseline, không phải lỗi do URP:

- `LevelLoader.OnEnable` đăng ký ba listener, không thấy phần gỡ tương ứng trong class. Kiểm tra cơ chế BaseMono/event trước khi kết luận có leak; test disable/enable lặp.
- `LevelLoader.Start` gọi async `LoadLevel()` mà không await; luồng tải level và popup cần kiểm tra lỗi load/handle ownership. Chưa thấy release handle tương ứng trong các class tải asset đã đọc.
- `SceneLoader.ChangeScene` gọi unload rồi bắt đầu load ngay; có nguy cơ chồng luồng khi đổi scene liên tiếp. Nếu dùng camera stack, chỉ đăng ký/gỡ camera theo thời điểm load/unload hoàn tất.
- `LoadingManager.LoadScene` có `async void`, chờ progress/remote config không có timeout hoặc cancellation ngay trong method. Cần phân biệt kẹt initialization với mất hình do pipeline.
- `EditorInitializer.cs` tự chuyển về Launcher khi bấm Play (mặc định bật). Khi test scene riêng phải ghi nhận trạng thái menu `The Beginning/Auto Switch Launcher Scene` và khôi phục sau test; luồng này không thay cho test bootstrap của player.

## 5. Cấu hình URP đề xuất ban đầu

- Giữ Unity 6000.0.68f1 trong đợt chuyển, cài `com.unity.render-pipelines.universal` dòng 17.0.x tương thích Unity 6000.0 từ Package Manager. Chốt patch thực sự được Editor resolve và lưu manifest/lock; không dùng một patch suy đoán hoặc nâng Unity đồng thời.
- Tạo `Assets/_Project/Settings/Rendering/URP_Mobile.asset` và Universal Renderer Data trong cùng thư mục. Forward, SRP Batcher bật; camera stack dùng chung renderer.
- Gán URP asset ở Graphics và mức Quality `Default`; kiểm tra mọi Build Profile/quality override khi đổi target.
- Giữ Gamma trong bước so sánh đầu để hạn chế sai khác màu. Nếu cần Linear, làm thành bước riêng có ảnh đối chiếu, nhất là Spine/PMA và particle blending.
- Render Scale 1.0; MSAA 2x làm mốc so sánh. HDR, Depth Texture, Opaque Texture, post-processing mặc định tắt nếu prototype chứng minh không có nội dung cần chúng. Camera Launcher đang bật HDR, nên phải kiểm tra ảnh trước khi chốt tắt.
- Chỉ bật shadow/additional lights theo nội dung thực sự; thiết lập shadow trong Quality Built-in không tự biểu diễn đầy đủ cấu hình URP asset.
- Không thêm Renderer Feature nếu không có nhu cầu. Với URP 17, dùng Render Graph cho feature mới; Compatibility Mode chỉ là phương án tạm khi có dependency bắt buộc đã xác định.

## 6. Kế hoạch thực hiện và điểm nghiệm thu

| Bước | Công việc | Đầu ra / điều kiện hoàn thành |
|---|---|---|
| 0. Baseline | Tạo nhánh riêng; lưu trạng thái Packages/Settings/assets; xử lý bootstrap sai; kiểm tra fallback shader; build Built-in | Player cold start thành công, ảnh tham chiếu UI/VFX và log baseline |
| 1. Kiểm kê sâu | Quét material và toàn bộ subasset, dependency scene/prefab/Addressables; liệt kê camera runtime, Spine và shader variants | Danh sách asset cần đổi, asset giữ nguyên và asset chưa sử dụng |
| 2. Prototype URP | Cài package; tạo pipeline/renderer; gán Graphics/Quality; thử Launcher trước rồi Service + Game | Không lỗi compile/shader; camera và canvas hiển thị đúng |
| 3. Camera | Chốt Base/Overlay; xử lý vòng đời stack trong scene loading nếu cần | Load/unload/replay không mất UI, không giữ camera chết, thứ tự đúng |
| 4. Material | Chỉ chạy Render Pipeline Converter trên nhóm phù hợp sau khi preview; xử lý UI/TMP/Spine thủ công theo danh sách | Không material hồng, mask/blend/SDF/VFX đúng; diff có thể review |
| 5. Nội dung | Clean build Addressables từng target; xác minh shader variants và shared dependencies | Test bằng Use Existing Build và player với bộ nội dung mới |
| 6. Thiết bị | Build Android và iOS; test cold start, UI, VFX, scene, pause/resume và hiệu năng | Đạt ma trận test bên dưới; ghi kết quả thiết bị/API |
| 7. Chốt | Review diff, pin versions, cập nhật tài liệu và lưu bản build baseline | Bộ thay đổi và đường rollback đầy đủ |

Ước lượng sơ bộ: 3–5 ngày công gồm baseline/kiểm kê 0.5–1 ngày, cấu hình/camera 0.5–1 ngày, shader/UI 1–2 ngày, build/QA 1 ngày. Cần điều chỉnh nếu phát hiện Spine world-space, shader ẩn hoặc lỗi bootstrap/SDK ngoài phạm vi rendering.

## 7. Ma trận kiểm thử

- **Cold start:** Entry → Launcher → Service + Game từ player; không chỉ Play Launcher. Kiểm tra load thất bại, retry và mạng chậm nếu luồng thực tế phụ thuộc dịch vụ.
- **UI:** home, in-game, win/lose, shop, IAP, setting, daily reward, leaderboard, update, notification và debug overlay. Kiểm tra camera của canvas, sorting, input, mask/RectMask2D, chữ tiếng Việt/fallback/outline/shadow, shiny và particle additive.
- **Gameplay:** cả ba level Addressable, replay, next, back/home; đổi scene nhiều lần; theo dõi camera count, draw calls và tài nguyên không tăng vô hạn.
- **VFX/Spine:** pool spawn/despawn, hiệu ứng rank/star; PMA/straight alpha và tint nếu Spine thực sự được dùng.
- **Nội dung:** Use Existing Build, fresh install với bundle mới, không phụ thuộc cache Editor; kiểm tra player shader stripping và shader variant cần thiết.
- **Thiết bị:** Android cấu hình thấp và thiết bị đại diện; iOS trên thiết bị thực với Metal khi có môi trường build. Dùng đúng graphics API đang cấu hình ở từng target, ghi rõ API/thiết bị trong kết quả.
- **Hiệu năng:** cùng thiết bị, độ phân giải, scene và thời lượng warm-up; so CPU/GPU frame time, FPS ổn định, peak memory, GC, draw calls, thời gian tải và kích thước player/bundle. Frame budget 16.7 ms nếu mục tiêu 60 FPS hoặc 33.3 ms nếu 30 FPS; chốt mục tiêu sản phẩm trước khi nghiệm thu.

Điều kiện pass: không lỗi compile/shader mới; không material hồng; mọi màn chính và thứ tự UI tương đương baseline; không mất nội dung sau scene unload; cold start player chạy đúng; Addressables tải đủ; không hồi quy hiệu năng đáng kể so baseline theo ngưỡng nhóm dự án chốt. Hiện chưa có số đo để đặt ngưỡng phần trăm chính xác.

## 8. Rollback và giới hạn hiện tại

Tách commit baseline fixes, package/settings, camera/C#, material và nội dung build. Khi rollback, khôi phục đồng bộ manifest/lock, pipeline settings, scene/prefab/material/C# và bộ Addressables/player cùng phiên bản. Chỉ bỏ pipeline asset không khôi phục được material đã convert.

Chưa thực hiện migration, test hình ảnh URP, chạy runtime, đo performance, build player hay xác minh trên thiết bị. Coplay chỉ xác nhận Editor hiện compile sạch và trạng thái Built-in; các kết luận về tương thích shader/camera URP là hạng mục cần kiểm chứng theo kế hoạch. Những vấn đề runtime C# nêu trên là rủi ro từ mã nguồn, chưa được tái hiện.
