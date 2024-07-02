//
//  LPMInitializer.m
//  iOSBridge
//
// Copyright © 2024 IronSource. All rights reserved.
//

#import "LPMInitializer.h"

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

#ifdef __cplusplus
extern "C" {
#endif
    
    void UnitySendMessage(const char* obj, const char* method, const char* msg);
    
    void LPMInitialize(const char *appKey, const char *userId, const char **adFormats) {
        NSMutableArray *formatsArray = [NSMutableArray array];
        const char **current = adFormats;
        if(current != NULL){
            while (*current != NULL) {
                NSString *format = GetStringParam(*current);
                if (format) {
                    [formatsArray addObject:format];
                }
                current++;
            }
        }
        [[LPMInitializer sharedInstance] LPMInitialize:GetStringParam(appKey)
                                                userId:GetStringParam(userId)
                                             adFormats:formatsArray];
    }
    
    
#ifdef __cplusplus
}
#endif

@implementation LPMInitializer

+ (instancetype)sharedInstance {
    static LPMInitializer *sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[self alloc] init];
    });
    return sharedInstance;
}

- (void)LPMInitialize:(NSString *)appKey userId:(NSString *)userId adFormats:(NSArray *)adFormats {
    
    LPMInitRequestBuilder *requestBuilder = [[LPMInitRequestBuilder alloc] initWithAppKey:appKey];
    [requestBuilder withUserId:userId];
    [requestBuilder withLegacyAdFormats:adFormats];
    LPMInitRequest *request = [requestBuilder build];
    
    [LevelPlay initWithRequest:request completion:^(LPMConfiguration * _Nullable config, NSError * _Nullable error) {
        if (error) {
            [self initializationDidFailWithError:error];
        } else {
            [self initializationDidCompleteWithConfiguration: config];
        }
    }];
}

- (NSString *)serializeConfigToJSON:(LPMConfiguration *)config {
    NSDictionary *configDict = @{
        @"isAdQualityEnabled": config.isAdQualityEnabled == true ? @"true" : @"false"
    };
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:configDict options:0 error:&error];
    return jsonData ? [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding] : @"";
}

- (NSString *)serializeErrorToJSON:(NSError *)adError{
    NSLog(@"levelplay failed to load-3");
    NSDictionary *errorDict = @{
        @"errorCode": [@(adError.code) stringValue] ?: @"",
        @"errorMessage": adError.description ?: @""
    };
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:errorDict options:0 error:&error];
    return jsonData ? [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding] : @"";
}

- (void)initializationDidCompleteWithConfiguration:(LPMConfiguration *)config {
    NSString *jsonString = [self serializeConfigToJSON:config];
    const char *configString = [jsonString UTF8String];
    UnitySendMessage("IosLevelPlaySdk", "OnInitializationSuccess", configString);
}

- (void)initializationDidFailWithError:(NSError *)error {
    NSString *jsonString = [self serializeErrorToJSON:error];
    const char *errorString = [jsonString UTF8String];
    UnitySendMessage("IosLevelPlaySdk", "OnInitializationFailed", errorString);
}

@end
