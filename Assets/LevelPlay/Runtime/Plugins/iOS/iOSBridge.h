//
//  iOSBridge.h
//  iOSBridge
//
//  Created by Supersonic.
//  Copyright (c) 2015 Supersonic. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <IronSource/IronSource.h>
#import "RewardedVideoLevelPlayCallbacksWrapper.h"
#import "InterstitialLevelPlayCallbacksWrapper.h"
#import "BannerLevelPlayCallbacksWrapper.h"

static NSString *  UnityGitHash = @"da76eac";
typedef void (*ISUnityBackgroundCallback)(const char* args);
typedef void (*ISUnityPauseGame)(const bool gamePause);

@interface iOSBridge : NSObject<ISSegmentDelegate,
								ISImpressionDataDelegate,
								ISConsentViewDelegate,
								ISInitializationDelegate>

@end


