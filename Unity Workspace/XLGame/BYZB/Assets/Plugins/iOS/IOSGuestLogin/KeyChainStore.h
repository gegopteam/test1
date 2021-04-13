//
//  KeyChainStore.h
//  Unity-iPhone
//
//  Created by tianGe on 2017/11/30.
//
//

#ifndef KeyChainStore_h
#define KeyChainStore_h

#include <stdio.h>
#import <Foundation/Foundation.h>

@interface KeyChainStore : NSObject

+ (void)save:(NSString *)service data:(id)data;

+ (id)load:(NSString *)service;

+ (void)deleteKeyData:(NSString *)service;

@end

#endif /* KeyChainStore_h */
