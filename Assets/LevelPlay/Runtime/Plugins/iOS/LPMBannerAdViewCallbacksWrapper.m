//
//  LPMBannerAdViewCallbacksWrapper.m
//  iOSBridge
//
// Copyright © 2024 IronSource. All rights reserved.
//

#import "LPMBannerAdViewCallbacksWrapper.h"

@implementation LPMBannerAdViewCallbacksWrapper

void *LPMBannerAdViewDelegateCreate(void* bannerAdPtr, DidLoadAdWithAdInfo loadSuccessCallback, DidFailToLoadAdWithAdUnitId loadFailedCallback, DidClickAdWithAdInfo clickedCallback, DidDisplayAdWithAdInfo displayedCallback, DidFailToDisplayAdWithAdInfo failedDisplayCallback, DidExpandAdWithAdInfo expandedCallback, DidCollapseAdWithAdInfo collapsedcallback, DidLeaveAppWithAdInfo leftAppCallback) {
    LPMBannerAdViewCallbacksWrapper *delegateWrapper =
        [[LPMBannerAdViewCallbacksWrapper alloc]
        initWithSuccessCallback:loadSuccessCallback
        failCallback:loadFailedCallback
        clickedCallback:clickedCallback
        displayedCallback:displayedCallback
        failedDisplayCallback:failedDisplayCallback
        expandedCallback:expandedCallback
        collapsedCallback:collapsedcallback
        leftAppCallback:leftAppCallback
        BannerAd:bannerAdPtr];

    return (__bridge_retained void *)delegateWrapper;
}

void LPMBannerAdViewDelegateDestroy(void *delegateRef) {
    LPMBannerAdViewCallbacksWrapper *delegateWrapper = (__bridge_transfer LPMBannerAdViewCallbacksWrapper *)delegateRef;
    delegateWrapper.loadSuccess = nil;
    delegateWrapper.loadFail = nil;
    delegateWrapper.clicked = nil;
    delegateWrapper.displayed = nil;
    delegateWrapper.failedToDisplay = nil;
    delegateWrapper.expand = nil;
    delegateWrapper.collapse = nil;
    delegateWrapper.leaveApp = nil;
    delegateWrapper.bannerAd = nil;
    }

 - (instancetype)initWithSuccessCallback:(DidLoadAdWithAdInfo)loadSuccessCallback failCallback:(DidFailToLoadAdWithAdUnitId)loadFailedCallback clickedCallback:(DidClickAdWithAdInfo)clickedCallback displayedCallback:(DidDisplayAdWithAdInfo)displayedCallback failedDisplayCallback:(DidFailToDisplayAdWithAdInfo)failedDisplayCallback expandedCallback:(DidExpandAdWithAdInfo)expandedCallback collapsedCallback:(DidCollapseAdWithAdInfo)collapsedCallback leftAppCallback:(DidLeaveAppWithAdInfo)leftAppCallback BannerAd:(void *)bannerAd{
     self = [super init];
     if (self) {
         self.loadSuccess = loadSuccessCallback;
         self.loadFail = loadFailedCallback;
         self.clicked = clickedCallback;
         self.displayed = displayedCallback;
         self.failedToDisplay = failedDisplayCallback;
         self.expand = expandedCallback;
         self.collapse = collapsedCallback;         
         self.leaveApp = leftAppCallback;
         self.bannerAd = bannerAd;
     }
     return self;
 }

// LPMAdInfo conversion methods
- (NSString *)serializeAdInfoToJSON:(LPMAdInfo *)adInfo {
    NSDictionary *adInfoDict = @{
        @"adUnitId": adInfo.adUnitId ?: @"",
        @"adSize": [self serializeAdSizeToJSON:adInfo.adSize],
        @"adFormat": adInfo.adFormat ?: @"",
        @"placementName": adInfo.placementName ?: @"",
        @"auctionId": adInfo.auction_id ?: @"",
        @"country": adInfo.country ?: @"",
        @"ab": adInfo.ab ?: @"",
        @"segmentName": adInfo.segment_name ?: @"",
        @"adNetwork": adInfo.ad_network ?: @"",
        @"instanceName": adInfo.instance_name ?: @"",
        @"instanceId": adInfo.instance_id ?: @"",
        @"revenue": adInfo.revenue ?: @"",
        @"precision": adInfo.precision ?: @"",
        @"encryptedCPM": adInfo.encrypted_cpm ?: @""
    };
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:adInfoDict options:0 error:&error];
    return jsonData ? [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding] : @"";
}

- (NSString *)serializeAdSizeToJSON:(LPMAdSize *)adSize {
    NSDictionary *adSizeDict = @{
        @"description": adSize.sizeDescription ?: @"",
        @"width": @(adSize.width) ?: @0,
        @"height": @(adSize.height) ?: @0
    };
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:adSizeDict options:0 error:&error];
    return jsonData ? [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding] : @"";
}

- (NSString *)serializeErrorToJSON:(NSError *)adError AdUnitId:(NSString *)adUnitId{
    NSDictionary *errorDict = @{
        @"errorCode": [@(adError.code) stringValue] ?: @"",
        @"errorMessage": adError.description ?: @"",
        @"adUnitId": adUnitId ?: @""
    };
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:errorDict options:0 error:&error];
    return jsonData ? [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding] : @"";
}

#pragma mark - LPMBannerAdViewDelegate Methods

- (void)didLoadAdWithAdInfo:(LPMAdInfo *)adInfo {
    NSString *jsonString = [self serializeAdInfoToJSON:adInfo];
    const char *adInfoString = [jsonString UTF8String];
    if (self.loadSuccess) {
        self.loadSuccess( self.bannerAd, adInfoString);
    }

}

- (void)didFailToLoadAdWithAdUnitId:(NSString *)adUnitId error:(NSError *)error {
    NSString *jsonString = [self serializeErrorToJSON:error AdUnitId:adUnitId];
    const char *errorString = [jsonString UTF8String];
    if (self.loadFail){
        self.loadFail(self.bannerAd, errorString);
    }
}

- (void)didClickAdWithAdInfo:(LPMAdInfo *)adInfo {
    NSString *jsonString = [self serializeAdInfoToJSON:adInfo];
    const char *adInfoString = [jsonString UTF8String];
   if (self.clicked) {
         self.clicked( self.bannerAd, adInfoString);
     }
}

- (void)didDisplayAdWithAdInfo:(LPMAdInfo *)adInfo {
    NSString *jsonString = [self serializeAdInfoToJSON:adInfo];
    const char *adInfoString = [jsonString UTF8String];
  if (self.displayed) {
           self.displayed(self.bannerAd, adInfoString);
       }
}

- (void)didFailToDisplayAdWithAdInfo:(LPMAdInfo *)adInfo error:(NSError *)error {
    NSString *jsonString = [self serializeAdInfoToJSON:adInfo];
    const char *adInfoString = [jsonString UTF8String];

    NSString *jsonStringError = [self serializeErrorToJSON:error AdUnitId:nil];
    const char *errorString = [jsonStringError UTF8String];
    if(self.failedToDisplay) {
        self.failedToDisplay(self.bannerAd, adInfoString, errorString);
    }
}

- (void)didExpandAdWithAdInfo:(LPMAdInfo *)adInfo {
    NSString *jsonString = [self serializeAdInfoToJSON:adInfo];
    const char *adInfoString = [jsonString UTF8String];
    if(self.expand) {
        self.expand(self.bannerAd, adInfoString);
    }
}

- (void)didCollapseAdWithAdInfo:(LPMAdInfo *)adInfo {
    NSString *jsonString = [self serializeAdInfoToJSON:adInfo];
    const char *adInfoString = [jsonString UTF8String];
    if(self.collapse) {
        self.collapse(self.bannerAd, adInfoString);
    }
}

- (void)didLeaveAppWithAdInfo:(LPMAdInfo *)adInfo {
    NSString *jsonString = [self serializeAdInfoToJSON:adInfo];
    const char *adInfoString = [jsonString UTF8String];
    if(self.leaveApp) {
        self.leaveApp(self.bannerAd, adInfoString);
    }
}

@end
