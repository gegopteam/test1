//
//  PurchaseIOSVerify.h
//  InAppPurchase
//
//  Created by zkinsen on 2017/8/23.
//  Copyright © 2017年 KinSenZ. All rights reserved.
//  负责：App内购验证

#import <Foundation/Foundation.h>

@interface PurchaseIOSVerify : NSObject

+ (id) getInstance;

- (bool) verifyTransactionReceipt: (NSString*) receipt;

- (bool) verifyTransactionReceipt;

@end
