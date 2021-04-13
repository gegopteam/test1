//
//  PurchaseIOSVerify.m
//  InAppPurchase
//
//  Created by zkinsen on 2017/8/23.
//  Copyright © 2017年 KinSenZ. All rights reserved.
//

#import "PurchaseIOSVerify.h"

#define IsSandbox

static PurchaseIOSVerify *purchaseIOSVerify = nil;

@implementation PurchaseIOSVerify

+ (id) getInstance
{
    static dispatch_once_t dis;
    dispatch_once(&dis, ^{
        if(nil==purchaseIOSVerify)
        {
            purchaseIOSVerify = [[PurchaseIOSVerify alloc] init];
        }
    });
    return purchaseIOSVerify;
}

- (instancetype) init
{
    self = [super init];

    return self;
}

//沙盒 https://sandbox.itunes.apple.com/verifyReceipt
//正式 https://buy.itunes.apple.com/verifyReceipt
- (bool) verifyTransactionReceipt: (NSString*) receipt
{
    NSDictionary *requestContents = @{
                                      @"receipt-data": receipt
                                      };
    NSError *error;
    //转换为JSON格式
    NSData *requestData = [NSJSONSerialization dataWithJSONObject:requestContents
                                                          options:0
                                                            error:&error];
    //不存在
    if(!requestData)
    {
        NSLog(@"不存在");
    }
    
    NSString *verifyUrl = nil;
#ifdef IsSandbox
    verifyUrl = @"https://sandbox.itunes.apple.com/verifyReceipt";
#else
    verifyUrl = @"https://buy.itunes.apple.com/verifyReceipt";
#endif
     verifyUrl = @"https://buy.itunes.apple.com/verifyReceipt";
    //国内访问苹果服务器比较慢，timeoutInterval需要长一点
    NSMutableURLRequest *storeRequest = [NSMutableURLRequest requestWithURL:[[NSURL alloc] initWithString:verifyUrl] cachePolicy:NSURLRequestUseProtocolCachePolicy timeoutInterval:10.0f];
    
    [storeRequest setHTTPMethod:@"POST"];
    [storeRequest setHTTPBody:requestData];
    
    NSString *result = [[NSString alloc] initWithData:requestData  encoding:NSUTF8StringEncoding];
    NSLog(@"Data:%@", result);
    
    // 在后台对列中提交验证请求，并获得官方的验证JSON结果
    NSOperationQueue *queue = [[NSOperationQueue alloc] init];
    [NSURLConnection sendAsynchronousRequest:storeRequest queue:queue
                           completionHandler:^(NSURLResponse *response, NSData *data, NSError *connectionError) {
                               if (connectionError) {
                                   NSLog(@"----------链接失败");
                               } else {
                                   NSError *error;
                                   NSDictionary *jsonResponse = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
                                   if (!jsonResponse) {
                                       NSLog(@"--------------验证失败");
                                   }
                                   
                                   for (NSString *key in jsonResponse) {
                                       NSLog(@"key: %@ value: %@", key, jsonResponse[key]);
                                   }
                                   
                                   // 比对 jsonResponse 中以下信息基本上可以保证数据安全
                                   /*
                                    bundle_id
                                    application_version
                                    product_id
                                    transaction_id
                                    */
                                   
                                   NSLog(@"-------------验证成功");
                               }
                           }];
    return false;
}

- (bool) verifyTransactionReceipt
{
    //验证凭证，获取拼过返回的交易凭证
    //appStoreReceiptURL IOS7.0添加的，购买交易完成后，会将凭证存放在在该地址
    NSURL *receiptURL = [[NSBundle mainBundle] appStoreReceiptURL];
    //从沙盒中获取到购买凭证
    NSData *receiptData = [NSData dataWithContentsOfURL:receiptURL];
    //传输的是BASE64编码的字符串
    /*
     BASE64 常用的编码方案，通常用于数据传输，以及加密算法的基础算法，传输过程中能够保证数据传输的稳定性
     BASE64 是可以编码和解码的
     */
    NSDictionary *requestContents = @{
                                      @"receipt-data": [receiptData base64EncodedStringWithOptions:0]
                                      };
    NSError *error;
    //转换为JSON格式
    NSData *requestData = [NSJSONSerialization dataWithJSONObject:requestContents
                                                          options:0
                                                            error:&error];
    //不存在
    if(!requestData)
    {
        NSLog(@"不存在");
    }
    
    NSString *verifyUrl = nil;
#ifdef IsSandbox
    verifyUrl = @"https://sandbox.itunes.apple.com/verifyReceipt";
#else
    verifyUrl = @"https://buy.itunes.apple.com/verifyReceipt";
#endif
    
    //国内访问苹果服务器比较慢，timeoutInterval需要长一点
    NSMutableURLRequest *storeRequest = [NSMutableURLRequest requestWithURL:[[NSURL alloc] initWithString:verifyUrl] cachePolicy:NSURLRequestUseProtocolCachePolicy timeoutInterval:10.0f];
    
    [storeRequest setHTTPMethod:@"POST"];
    [storeRequest setHTTPBody:requestData];
    
    // 在后台对列中提交验证请求，并获得官方的验证JSON结果
    NSOperationQueue *queue = [[NSOperationQueue alloc] init];
    [NSURLConnection sendAsynchronousRequest:storeRequest queue:queue
                           completionHandler:^(NSURLResponse *response, NSData *data, NSError *connectionError) {
                               if (connectionError) {
                                   NSLog(@"链接失败");
                               } else {
                                   NSError *error;
                                   NSDictionary *jsonResponse = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
                                   if (!jsonResponse) {
                                       NSLog(@"验证失败");
                                   }
                                   
                                   for (NSString *key in jsonResponse) {
                                       NSLog(@"key: %@ value: %@", key, jsonResponse[key]);
                                   }
                                   
                                   // 比对 jsonResponse 中以下信息基本上可以保证数据安全
                                   /*
                                    bundle_id
                                    application_version
                                    product_id
                                    transaction_id
                                    */
                                   
                                   NSLog(@"验证成功");
                               }
                           }];
    
    return false;
}

@end
