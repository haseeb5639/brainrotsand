#import "NativeAppstore.h"
@implementation NativeAppstore

- (id)init
{
    self = [super init];
    
    return self;
}
- (void)showOverlayForAppWithId:(long)appId
{
    
    NSString *str = [NSString stringWithFormat:@"%ld",appId];
    if (@available(iOS 14.0, *)) {
        SKOverlayAppConfiguration *config = [[SKOverlayAppConfiguration alloc] initWithAppIdentifier:str
                                                                                            position:SKOverlayPositionBottom];

        SKOverlay *overlay = [[SKOverlay alloc] initWithConfiguration:config];
        UIWindow *window = UIApplication.sharedApplication.windows.firstObject;
        [overlay presentInScene:window.windowScene];
    }
}
-(void) openAppInStore: (long) appID
{
    if ([SKStoreProductViewController class])
    {
        NSDictionary *parameters = @{SKStoreProductParameterITunesItemIdentifier: [NSNumber numberWithInteger: appID]};
        
        SKStoreProductViewController *productViewController = [[SKStoreProductViewController alloc] init];
        [productViewController loadProductWithParameters:parameters completionBlock:nil];
        [productViewController setDelegate:self];
        [UnityGetGLViewController() presentViewController:productViewController animated:YES completion:nil];
    }
    
    else
    {
        
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString: [NSString stringWithFormat: @"http://itunes.apple.com/app/id%ld?mt=8", appID]]];
    }
}

-(void)productViewControllerDidFinish:(SKStoreProductViewController *)viewController
{   [viewController dismissViewControllerAnimated:YES completion:nil];
    
    
    UnitySendMessage("NativePopUpsManager", "appstoreClosed", "");
}

- (BOOL)prefersStatusBarHidden
{   return YES;
}

@end

static NativeAppstore *nativeAppstorePlugin = nil;

extern "C"
{
    void _OpenAppInStore(long appID)
    {
        NSLog(@"NativeAppStore :: Open App %ld", appID);
        
        if (nativeAppstorePlugin == nil)
            nativeAppstorePlugin = [[NativeAppstore alloc] init];
        
        [nativeAppstorePlugin openAppInStore: appID];
    }
void _ShowOverlayForAppWithId(long appID)
{
    NSLog(@"NativeAppStore :: Open App %ld", appID);
    
    if (nativeAppstorePlugin == nil)
        nativeAppstorePlugin = [[NativeAppstore alloc] init];
    
    [nativeAppstorePlugin showOverlayForAppWithId: appID];
}
}
