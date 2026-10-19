# Kết quả chuyển base sang URP

Ngày triển khai: 19/10/2026. Nhánh: `use_addressable_urp`.

## Trạng thái

Đã chuyển cấu hình rendering và camera của base sang URP 17.0.4 trên Unity 6000.0.68f1. Đã chạy kiểm tra trong Unity Editor qua Coplay MCP và quan sát ảnh Game View. Clean build Addressables Android đã thành công. Android development APK đã build thành công, 0 lỗi; iOS chưa được build.

Người dùng đã xác nhận chủ động gỡ Unity IAP; giữ package ở trạng thái đã gỡ. Không thêm Tangle giả hoặc bỏ qua xác thực receipt để né lỗi build.

## Thay đổi

- Thêm `com.unity.render-pipelines.universal: 17.0.4` và dependencies được Unity resolve trong package lock.
- Tạo `Assets/_Project/Settings/Rendering/URP_Mobile.asset` và `URP_Mobile_Renderer.asset`: Universal Renderer, Forward, Render Scale 1, MSAA 2x, SRP Batcher; HDR, Depth/Opaque Texture và post-processing tắt. Main light per-pixel, shadow distance 20; additional lights tắt. Giữ Gamma.
- Gán pipeline trong Graphics và Quality Default; Unity tạo URP Global Settings, Default Volume Profile và Shader Graph settings. Render Graph bật (`m_EnableRenderCompatibilityMode: 0` trong settings hiện hành; trường legacy `m_EnableRenderGraph` không dùng để kết luận).
- Thêm `UrpCameraStack` trên camera Launcher, Game, Service và prefab `DefaultEnviroment`. Camera Launcher/môi trường là Base; UI Game/Service là Overlay theo thứ tự depth. Khi không có Base môi trường, UI đầu tiên làm Base dự phòng. Đăng ký/gỡ camera qua OnEnable/OnDisable để hỗ trợ scene additive và prefab runtime.
- Sửa đường dẫn scene player trong Build Settings từ `Initialization.unity` về `Entry.unity`, giữ GUID hiện có.
- Đăng ký lại group Launcher trong Addressables Settings: file group có sẵn nhưng thiếu trong danh sách `m_GroupAssets`, gây `No Location found for Key=Launcher` khi chạy từ Entry.
- Sửa fallback legacy UIParticle từ tên shader không tồn tại `UI Extensions/Particles/Additive` thành `UI/Additive` của package UIParticle đang có. Thêm shader này vào Always Included để hỗ trợ đường Shader.Find trong player.
- Giữ shader material UGUI/TMP/Shiny/Additive hiện có, không thay bằng URP/Lit hàng loạt.

Các script migration/build tạm chỉ dùng trong quá trình thao tác đã được dọn, không yêu cầu chạy lại để sử dụng base. Thay đổi serialize bổ sung của Unity 6/TMP/Sunflower xuất hiện khi lưu scene.

## Kiểm tra đã thực hiện

| Kiểm tra | Kết quả |
|---|---|
| Unity Editor compile sau cấu hình | Coplay trả `No compile errors` |
| Pipeline thực tế và Quality | Cả hai là `URP_Mobile` |
| Material, bao gồm subasset | Nhóm material UI/TMP/VFX/Shiny được khảo sát không có shader error tại thời điểm kiểm tra |
| Luồng Play từ Launcher | Tải Service + Game và PopupHome thành công bằng Use Asset Database |
| Camera stack khi chỉ có UI | Camera_UI Base → CameraUI_Service Overlay |
| Thêm camera môi trường runtime | Camera môi trường Base → Camera_UI → CameraUI_Service |
| Vòng đời camera | Pass khi Service tắt/bật, môi trường tắt/bật/hủy; UI Base dự phòng được phục hồi và stack không giữ camera đã hủy |
| Material lỗi gắn lên nội dung runtime | Không có Renderer hoặc UI Graphic dùng material lỗi trong các màn được khảo sát |
| Launcher/Home/in-game | Đã quan sát ảnh Game View; UI và chữ hiển thị trên URP |
| Clean Addressables Android sau khi gỡ IAP | Thành công, 594.36 giây; output `Library/com.unity.addressables/aa/Android/settings.json` |
| Bootstrap Entry + Use Existing Build | Tải Service + Game + PopupHome; stack UI hoạt động |
| Android development player | Thành công, 0 lỗi, 421.52 giây; APK khoảng 97 MiB, development/debug signing |
| Compile Editor sau build và khôi phục workspace | Coplay trả `No compile errors` |

Ảnh Launcher và Home đã quan sát:

- [Launcher](images/urp-launcher.png)
- [Home](images/urp-home.png)

Coplay `capture_ui_canvas` trả ảnh đen với cấu hình này; đã dùng ảnh Game View qua ScreenCapture để xác minh. Một material InternalErrorShader nội bộ có trong Resources, nhưng không được gắn lên Renderer/Graphic trong các màn được kiểm tra.

## Giới hạn và việc còn cần xác nhận

- Coplay timeout trong build dài không đồng nghĩa build thất bại: clean Addressables và Android APK đã được xác nhận qua file kết quả và file APK. Log cũng có lỗi TLS ngoài rendering.
- APK kiểm thử tại `Builds/URP/TheBeginning-URP.apk` (thư mục Builds được gitignore). Lần đầu custom signing thiếu password; người dùng nhập password trong lúc bản debug signing đã chạy. Sau build đã khôi phục cấu hình custom keystore. Đây là bản development để kiểm thử, chưa phải bản ký phát hành.
- Đã khôi phục scene Launcher, bật lại Auto Switch Launcher Scene, Use Asset Database và play-mode reload options ban đầu; đã bỏ các thay đổi dynamic font/scene bootstrap chỉ phát sinh khi test/build.
- Đã kiểm tra Entry bằng Use Existing Build sau khi đăng ký lại Launcher. Unity cảnh báo bundle Android không hoàn toàn tương thích Editor: các shader chỉ compile cho Android có thể unsupported trên macOS Editor. Việc này không thay thế nghiệm thu trên thiết bị.
- In-game có cảnh báo missing scripts ở LevelText, Btn_Replay và Btn_Home trong prefab cũ. Chưa sửa logic/template ngoài phạm vi rendering; cần rà soát GUID/component trước nghiệm thu gameplay đầy đủ.
- Chưa đo CPU/GPU/memory trên thiết bị, chưa xác minh mọi popup/VFX/mask/Spine hoặc build iOS. Không khẳng định tương đương hình ảnh toàn bộ hay tăng hiệu năng.

## Cách sử dụng và kiểm tra tiếp

1. Mở bằng Unity 6000.0.68f1; đợi resolve package/import hoàn tất. Graphics và Quality phải trỏ `URP_Mobile`.
2. Chạy Launcher để kiểm tra UI nhanh. Kiểm tra từ Entry khi tắt tạm `The Beginning/Auto Switch Launcher Scene` để editor helper không bỏ qua bootstrap.
3. Mở Addressables Groups và xác nhận Launcher, Service, Game, Levels có trong settings tại `Assets/ThirdParty/AddressableAssetsData`.
4. Khi thay nội dung hoặc cần build lại: `Build > Clean Build > All`, rồi `Build > New Build > Default Build Script` trong Addressables Groups cho Android.
5. Đã kiểm tra Use Existing Build và tạo APK development. Cài APK lên thiết bị Android để nghiệm thu cold start Entry, shader stripping và hình ảnh. Build lại nội dung riêng cho iOS trước khi build player iOS.
6. Kiểm tra popup/UI particles, disable/enable camera, replay/next/home và pause/resume; tham khảo ma trận chi tiết trong [kế hoạch](URP_Migration_Plan.md).

Khi thêm camera mới: gán Universal Additional Camera Data, dùng renderer index 0 và `UrpCameraStack`; camera nội dung chọn Base (`overlay = false`), camera UI chồng lên chọn `overlay = true`. Registry hiện quản lý một chuỗi camera cho màn hình chính; không dùng cho nhiều màn hình hoặc RenderTexture độc lập mà chưa mở rộng thiết kế.

## Rollback

Khôi phục đồng bộ package manifest/lock, Graphics/Quality, scene/prefab camera, component UrpCameraStack và nội dung Addressables của phiên bản trước. Các sửa bootstrap có thể giữ riêng vì xử lý lỗi có sẵn. Việc gỡ IAP là quyết định riêng đã được người dùng xác nhận. Chưa tạo commit hay push. Linker preservation file `Assets/ThirdParty/AddressableAssetsData/link.xml` do Addressables build tạo được giữ lại, bao gồm UrpCameraStack và các component tải động.
