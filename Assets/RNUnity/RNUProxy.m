#import "RNUProxy.h"

/** Implemented by react-native-unity2 to receive messages from Unity */
void (*RNUProxyEmitEvent_fun)(const char*, const char*) = NULL;

void RNUProxyEmitEvent(const char* name, const char* data) {
    if (RNUProxyEmitEvent_fun)
        (*RNUProxyEmitEvent_fun)(name, data);
    else
        NSLog(@"ERROR: RNUProxyEmitEvent_fun was not assigned!");
}
