#import "RNUProxy.h"

@interface RNUProxy : NSObject

+ (Class)RNUnity;

@end

@implementation RNUProxy

+ (Class)RNUnity {
    return NSClassFromString(@"RNUnity");
}

@end

void RNUProxyEmitEvent(const char* name, const char* data) {
    [RNUProxy.RNUnity emitEvent:name data:data];
}
