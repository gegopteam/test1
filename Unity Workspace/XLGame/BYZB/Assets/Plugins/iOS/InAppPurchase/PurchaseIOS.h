//
//  PurchaseIOS.h
//  InAppPurchase
//
//  Created by zkinsen on 2017/8/21.
//  Copyright © 2017年 KinSenZ. All rights reserved.
//  负责：App内购

#import <Foundation/Foundation.h>

#import <StoreKit/StoreKit.h>

#ifndef PRODUCT_ID
#define PRODUCT_ID
//在内购项目中创建的商品单号
//钻石
#define ProductID_Diamond_CNY_6 @"Diamond_CNY_6"
#define ProductID_Diamond_CNY_30 @"Diamond_CNY_30"
#define ProductID_Diamond_CNY_68 @"Diamond_CNY_68"
#define ProductID_Diamond_CNY_118 @"Diamond_CNY_118"
#define ProductID_Diamond_CNY_198 @"Diamond_CNY_198"
#define ProductID_Diamond_CNY_348 @"Diamond_CNY_348"
//#define ProductID_Diamond_CNY_128 @"Diamond_CNY_128"
//#define ProductID_Diamond_CNY_328 @"Diamond_CNY_328"
//#define ProductID_Diamond_CNY_648 @"Diamond_CNY_648"

//金币
//#define ProductID_Gold_CNY_6 @"Gold_CNY_6"
//#define ProductID_Gold_CNY_30 @"Gold_CNY_30"
//#define ProductID_Gold_CNY_68 @"Gold_CNY_68"
//#define ProductID_Gold_CNY_118 @"Gold_CNY_118"
//#define ProductID_Gold_CNY_198 @"Gold_CNY_198"
//#define ProductID_Gold_CNY_348 @"Gold_CNY_348"

#define PID_CL_Gold_6 @"CL_GOLD_6"
#define PID_CL_Gold_18 @"CL_GOLD_18"
#define PID_CL_Gold_50 @"CL_GOLD_50"
#define PID_CL_Gold_98 @"CL_GOLD_98"
#define PID_CL_Gold_198 @"CL_GOLD_198"
//#define PID_CL_Gold_298 @"CL_GOLD_298"
#define PID_CL_Gold_488 @"CL_GOLD_488"


//#define ProductID_Gold_CNY_128 @"Gold_CNY_128"
//#define ProductID_Gold_CNY_328 @"Gold_CNY_328"
//#define ProductID_Gold_CNY_648 @"Gold_CNY_648"
//卡
#define ProductID_Card_Month_CNY_28 @"Card_Month_CNY_28" //月卡
#define ProductID_Card_Room_CNY_6 @"Card_Room_CNY_6" //房卡
#define Pack_Preference_CNY_6 @"Pack_Preference_CNY_6" //特惠礼包
#endif

#ifndef PURCHASE_MSG
#define PURCHASE_MSG
//注：这边的编号需要跟Unity部分的统一
#define IOS_INIT_BACK 1
#define IOS_BUY_BACK 2

#endif



@interface PurchaseIOS : NSObject<SKPaymentTransactionObserver, SKProductsRequestDelegate>

@property NSString *unityObject;
@property NSString *unityMethod;


+ (id) getInstance;

//购买
//- (bool) payProductWithIndex:(int)index;
- (bool) payProductWithID:(NSString *)product;

- (void) showProducts;


@end
