//
//  PurchaseIOS.m
//  InAppPurchase
//
//  Created by zkinsen on 2017/8/21.
//  Copyright © 2017年 KinSenZ. All rights reserved.
//

#import "PurchaseIOS.h"
#import "PurchaseIOSVerify.h"



static PurchaseIOS *purchaseIOS = nil;

@interface PurchaseIOS ()

@property float systemVersion;
@property (nonatomic, strong) NSArray *storeProductsArray;

@end

@implementation PurchaseIOS

+ (id) getInstance
{
    static dispatch_once_t dis;
    dispatch_once(&dis, ^{
        if(nil==purchaseIOS)
        {
            purchaseIOS = [[PurchaseIOS alloc] init];
            if(nil!=purchaseIOS)
            {
                purchaseIOS.unityObject = [[NSString alloc] init];
                purchaseIOS.unityMethod = [[NSString alloc] init];
            }
        }
    });
    return purchaseIOS;
}

- (instancetype) init
{
    self.systemVersion = [[[UIDevice currentDevice] systemVersion] floatValue];
    if(self = [super init])
    {
        [self requestAppleProducts];
    }
    return self;
}

- (void) setTransactionObserver: (bool) add
{
    if(add)
    {
        [[SKPaymentQueue defaultQueue] addTransactionObserver:self];
    }
    else
    {
        [[SKPaymentQueue defaultQueue] removeTransactionObserver:self];
    }
    
}

//支付请求
//- (bool) payProductWithIndex:(int)index
//{
//    if(nil==self.storeProductsArray)
//        return false;
//    if (index < self.storeProductsArray.count) {
//        SKProduct *product = [self.storeProductsArray objectAtIndex:index];
//        if(nil!=product)
//        {
//            NSLog(@"product:%d",index);
//            NSLog(@"price:%@",product.localizedTitle);
//            NSLog(@"price:%@",product.price);
//            
//            SKPayment *payment = [SKPayment paymentWithProduct:product];
//            [[SKPaymentQueue defaultQueue] addPayment:payment];
//            return true;
//        }
//    }
//    return false;
//}

- (bool) payProductWithID:(NSString *)productID
{
    
    NSLog( @"[ *********** ]start to pay apple product id %@" , productID );
    
    if(nil==self.storeProductsArray || nil==productID)
        return false;
    
    NSLog( @"[ *********** ]start to pay apple product id1 %@" , productID );
    
    for (SKProduct *product in self.storeProductsArray) {
        if ([productID isEqualToString: product.productIdentifier]) {
            SKPayment *payment = [SKPayment paymentWithProduct:product];
            NSLog( @"[ *********** ]start to pay apple product id2 %@" , productID );
            [[SKPaymentQueue defaultQueue] addPayment:payment];
            return true;
        }
    }
    
    return false;
}

//判断是否允许程序内付费购买
- (BOOL) canMakePayments
{
    return [SKPaymentQueue canMakePayments];
}

//请求单个产品信息
- (void)requestAppleProduct: (NSString *) productID
{
    NSSet *productIdentifiers = [NSSet setWithObject:productID];
    SKProductsRequest* productsRequest = [[SKProductsRequest alloc] initWithProductIdentifiers:productIdentifiers];
    productsRequest.delegate = self;
    [productsRequest start];
}

//请求产品信息
- (void)requestAppleProducts
{
    if(FALSE==[self canMakePayments])
    {
        NSLog(@"不允许程序内付费购买");
        return;
    }
    else
    {
        NSLog(@"允许程序内付费购买");
    }
    
    if ([SKPaymentQueue canMakePayments])
    {
        NSMutableSet *nsSet = [NSMutableSet set];
        
        [nsSet addObject:ProductID_Diamond_CNY_6];//
        [nsSet addObject:ProductID_Diamond_CNY_30];//
        [nsSet addObject:ProductID_Diamond_CNY_68];//
        [nsSet addObject:ProductID_Diamond_CNY_118];//
        [nsSet addObject:ProductID_Diamond_CNY_198];//
        [nsSet addObject:ProductID_Diamond_CNY_348];//
//        [nsSet addObject:ProductID_Diamond_CNY_128];//
//        [nsSet addObject:ProductID_Diamond_CNY_328];//
//        [nsSet addObject:ProductID_Diamond_CNY_648];//
        
//        [nsSet addObject:ProductID_Gold_CNY_6];//
//        [nsSet addObject:ProductID_Gold_CNY_30];//
//        [nsSet addObject:ProductID_Gold_CNY_68];//
//        [nsSet addObject:ProductID_Gold_CNY_118];//
//        [nsSet addObject:ProductID_Gold_CNY_198];//
//        [nsSet addObject:ProductID_Gold_CNY_348];//
        
        
        [nsSet addObject:PID_CL_Gold_6];//
        [nsSet addObject:PID_CL_Gold_18];//
        [nsSet addObject:PID_CL_Gold_50];//
        [nsSet addObject:PID_CL_Gold_98];//
        [nsSet addObject:PID_CL_Gold_198];//
        [nsSet addObject:PID_CL_Gold_488];//
        
//        [nsSet addObject:PID_CL_Gold_298];//
//        [nsSet addObject:ProductID_Gold_CNY_648];//
        
        [nsSet addObject:ProductID_Card_Month_CNY_28];
        [nsSet addObject:ProductID_Card_Room_CNY_6];
        [nsSet addObject:Pack_Preference_CNY_6];
        
        SKProductsRequest *productsRequest = [[SKProductsRequest alloc] initWithProductIdentifiers:nsSet];
        productsRequest.delegate = self;
        [productsRequest start];
        
        UIWindow * window = [UIApplication sharedApplication].keyWindow;
        //[MBProgressHUD showHUDAddedTo:window animated:YES title:nil];
    }
    
    [[SKPaymentQueue defaultQueue] addTransactionObserver:self];
}

//请求产品信息的回复
- (void) productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response
{
    self.storeProductsArray = response.products;
    NSArray *productsArray = response.products;
    if(0==productsArray.count)
    {
        NSLog(@"没有请求的产品信息");
        return;
    }
    else
    {
        NSLog(@"-----------收到产品反馈信息--------------");
        NSArray *myProduct = response.products;
        NSLog(@"产品Product ID:%@",response.invalidProductIdentifiers);
        NSLog(@"产品付费数量: %d", (int)[myProduct count]);
        NSInteger i = 1;
        for(SKProduct *product in myProduct){
            NSLog(@"-------------%ld--------------", (long)i++);
            NSLog(@"product info");
            NSLog(@"SKProduct 描述信息%@", [product description]);
            NSLog(@"产品标题 %@" , product.localizedTitle);
            NSLog(@"产品描述信息: %@" , product.localizedDescription);
            NSLog(@"价格: %@" , product.price);
            NSLog(@"Product id: %@" , product.productIdentifier);
            
        }
        
        if([self.storeProductsArray count] > 0)
        {
            self.storeProductsArray = [self.storeProductsArray sortedArrayUsingComparator:
                                       ^NSComparisonResult(SKProduct* product1, SKProduct* product2)
                                       {
                                           return [product1.price compare: product2.price];
                                       }];
        }
        
    }
    
    return;
}

- (void) showProducts
{
    if(nil==self.storeProductsArray)
        return;
    NSLog(@"\n-------------排序--------------");
    
    int i=1;
    NSArray *myProduct = self.storeProductsArray;
    for(SKProduct *productTmp in myProduct){
        NSLog(@"-------------%ld--------------", (long)i++);
        NSLog(@"product info");
        NSLog(@"SKProduct 描述信息%@", [productTmp description]);
        NSLog(@"产品标题 %@" , productTmp.localizedTitle);
        NSLog(@"产品描述信息: %@" , productTmp.localizedDescription);
        NSLog(@"价格: %@" , productTmp.price);
        NSLog(@"Product id: %@" , productTmp.productIdentifier);
        
    }
}

- (void) requestDidFinish:(SKRequest *)request
{
    NSLog(@"----------请求产品信息结束--------------");
    [self InitToUnity];
//    if ([self.unityObject length] > 0 && [self.unityMethod length] > 0) {
//        NSString *data = [NSString stringWithFormat:@"{\\\"result\\\":0,\\\"data\\\":\\\"准备就绪\\\"}"];
//        NSString *content = [NSString stringWithFormat:@"{\"msg\":%d,\"data\":\"%@\"}", IOS_INIT_BACK, data];
//        UnitySendMessage([self.unityObject UTF8String], [self.unityMethod UTF8String], [content UTF8String]);
//    }
    
}

//弹出错误信息
- (void) request:(SKRequest *)request didFailWithError:(NSError *)error{
    NSLog(@"-------弹出错误信息----------");
    UIAlertView *alerView =  [[UIAlertView alloc] initWithTitle:NSLocalizedString(@"Alert",NULL) message:[error localizedDescription]
                                                       delegate:nil cancelButtonTitle:NSLocalizedString(@"Close",nil) otherButtonTitles:nil];
    [alerView show];
    
}

//-(void) PurchasedTransaction: (SKPaymentTransaction *)transaction{
//    NSLog(@"-----PurchasedTransaction----");
//    NSArray *transactions =[[NSArray alloc] initWithObjects:transaction, nil];
//    [self paymentQueue:[SKPaymentQueue defaultQueue] updatedTransactions:transactions];
//}

- (void) paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray<SKPaymentTransaction *> *)transactions
{
    
     NSLog(@"recv ------------paymentQueue--------------");
    
    for(SKPaymentTransaction *transaction in transactions)
    {
        switch (transaction.transactionState) {
            case SKPaymentTransactionStatePurchased: //交易完成
                NSLog(@"交易完成");
                [self completeTransaction:transaction];
                break;
            case SKPaymentTransactionStateFailed: //交易失败
                NSLog(@"交易失败");
                [self failedTransaction:transaction];
                break;
            case SKPaymentTransactionStateRestored: //已经购买过该商品
                NSLog(@"已经购买过该商品");
                [self restoreTransaction:transaction];
                break;
            case SKPaymentTransactionStatePurchasing: //商品添加进列表
                NSLog(@"商品添加进列表");
                break;
            default:
                break;
        }
    }
    
}

- (void) completeTransaction: (SKPaymentTransaction *) transaction
{
    /*NSString * str = [[NSString alloc]initWithData:transaction.transactionReceipt encoding:NSUTF8StringEncoding];
    NSData* dataJson = [str dataUsingEncoding:NSUTF8StringEncoding];
    NSError *error;
    //NSDictionary *jsonResponse = [NSJSONSerialization JSONObjectWithData: [str dataUsingEncoding:NSUTF8StringEncoding] options:0 error:&error];
    NSDictionary *jsonResponse = [NSJSONSerialization JSONObjectWithData: dataJson options:0 error:&error];
    if (!jsonResponse) {
        NSLog(@"验证失败");
    }
    
    for (NSString *key in jsonResponse) {
        NSLog(@"key: %@ value: %@", key, jsonResponse[key]);
    }
    
    //NSString *environment=[self environmentForReceipt:str];
    NSLog(@"----- 完成交易调用的方法 1--------%@",str);
    
    //-------------------------------------------------------------------
    
    NSString *productIdentifier = transaction.payment.productIdentifier;
    
    if([productIdentifier length] > 0)
    {//向自己的服务器验证购买凭证
        NSLog(@"产品信息：%@", productIdentifier);
        //注：客户端是否需要数据库，以防突发情况导致充值了，无法告知服务器充值的信息
        //需要做一个服务端验证程序，让服务端调用
    }
     
//    NSURL *receiptURL = [[NSBundle mainBundle] appStoreReceiptURL];
//    NSData *receiptData = [NSData dataWithContentsOfURL:receiptURL];
    //*
    //获取支付凭证
    */
    
    
   // [[PurchaseIOSVerify getInstance] verifyTransactionReceipt:transaction.transactionReceipt];
    
    //NSURL   *receiptUrl = [[NSBundle mainBundle] appStoreReceiptURL];
    //NSData  *receipt    =  [NSData dataWithContentsOfURL:receiptUrl];
    NSString *orderIdentifier = transaction.transactionIdentifier;
   // NSData *receiptData = nil;
    NSString *transactionReceipt = nil;
    transactionReceipt = [transaction.transactionReceipt base64EncodedStringWithOptions:NSDataBase64EncodingEndLineWithLineFeed];
    
    // NSString *base64String = [[transactionReceipt dataUsingEncoding:NSUTF8StringEncoding] base64EncodedStringWithOptions:NSDataBase64Encoding64CharacterLineLength];
   // transactionReceipt =base64String;
   // transactionReceipt = [ transaction.transactionReceipt base64EncodedStringWithOptions:NSDataBase64EncodingEndLineWithLineFeed ];
//    if(self.systemVersion >= 7.0)
//    {
//        NSURLRequest *appstoreRequest = [NSURLRequest requestWithURL:[[NSBundle mainBundle]appStoreReceiptURL]];
//        NSError *error = nil;
//        receiptData = [NSURLConnection sendSynchronousRequest:appstoreRequest returningResponse:nil error:&error];
//        transactionReceipt = [receiptData base64EncodedStringWithOptions:NSDataBase64EncodingEndLineWithLineFeed];
//        
//        NSLog(@"ReceiptData:%@\n", receiptData);
//    }
//    else
//    {
//        receiptData = transaction.transactionReceipt;
        //transactionReceipt = [receiptData base64EncodedString];
    
//    }

  //  NSLog(@"ReceiptData:%@\n", receiptData);
    NSLog(@"TransactionReceipt:%@\n", transactionReceipt);//*/
    
    //验证购买凭证
   //  [[PurchaseIOSVerify getInstance] verifyTransactionReceipt: [transaction.transactionReceipt base64EncodedStringWithOptions:NSDataBase64EncodingEndLineWithLineFeed]];

    // [[PurchaseIOSVerify getInstance] verifyTransactionReceipt: transactionReceipt];
    
    [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
    
    NSString *dataInfo = [NSString stringWithFormat:@"{\\\\\\\"orderId\\\\\\\":\\\\\\\"%@\\\\\\\",\\\\\\\"receipt\\\\\\\":\\\\\\\"%@\\\\\\\"}", orderIdentifier, transactionReceipt];
    [self BuyToUnity:0 Identifier: orderIdentifier Receipt: transactionReceipt];
    /*
    if ([self.unityObject length] > 0 && [self.unityMethod length] > 0) {
        NSLog(@"购买成功回复Unity");
        //transaction
        NSString *dataInfo = [NSString stringWithFormat:@"{\\\\\\\"orderId\\\\\\\":\\\\\\\"%@\\\\\\\",\\\\\\\"receipt\\\\\\\":\\\\\\\"%@\\\\\\\"}", orderIdentifier, transactionReceipt];
        NSString *data = [NSString stringWithFormat:@"{\\\"result\\\":%d,\\\"data\\\":\\\"%@\\\"}", 0, dataInfo];
        //NSString *data = [NSString stringWithFormat:@"\"{\\\"result\\\":%d,\\\"data\\\":%@}\"", 0, @"\\\"\\\""];
        NSString *content = [NSString stringWithFormat:@"{\"msg\":%d,\"data\":\"%@\"}", IOS_BUY_BACK, data];
        NSLog(@"return:%@", content);
        NSLog(@"unityObject:%@", self.unityObject);
        NSLog(@"unityMethod:%@", self.unityMethod);
        UnitySendMessage([self.unityObject UTF8String], [self.unityMethod UTF8String], [content UTF8String]);
    }//*/
    //UnitySendMessage("Canvas", "Fun", "。。。购买成功");
}

- (void) failedTransaction: (SKPaymentTransaction *) transaction
{
    if(SKErrorPaymentCancelled != transaction.error.code)
    {
        NSLog(@"[交易失败]");
        [self BuyToUnity:-1 Identifier: @"" Receipt: @""];
    }
    else
    {
        NSLog(@"[交易取消]");
        [self BuyToUnity:1 Identifier: @"" Receipt: @""];
    }
    [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
}

- (void) restoreTransaction:(SKPaymentTransaction *) transaction
{
    [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
}

- (void) InitToUnity
{
    if ([self.unityObject length] > 0 && [self.unityMethod length] > 0) {
        NSString *data = [NSString stringWithFormat:@"{\\\"result\\\":0,\\\"data\\\":\\\"准备就绪\\\"}"];
        NSString *content = [NSString stringWithFormat:@"{\"msg\":%d,\"data\":\"%@\"}", IOS_INIT_BACK, data];
        UnitySendMessage([self.unityObject UTF8String], [self.unityMethod UTF8String], [content UTF8String]);
    }
}

- (void) BuyToUnity: (int) result Identifier: (NSString *) orderIdentifier Receipt: (NSString *) transactionReceipt
{
    if (nil == orderIdentifier || nil == transactionReceipt)
        return;
    if ([self.unityObject length] > 0 && [self.unityMethod length] > 0) {
        NSString *dataInfo = [NSString stringWithFormat:@"{\\\\\\\"orderId\\\\\\\":\\\\\\\"%@\\\\\\\",\\\\\\\"receipt\\\\\\\":\\\\\\\"%@\\\\\\\"}", orderIdentifier, transactionReceipt];
        NSString *data = [NSString stringWithFormat:@"{\\\"result\\\":%d,\\\"data\\\":\\\"%@\\\"}", result, dataInfo];
        //NSString *data = [NSString stringWithFormat:@"\"{\\\"result\\\":%d,\\\"data\\\":%@}\"", 0, @"\\\"\\\""];
        NSString *content = [NSString stringWithFormat:@"{\"msg\":%d,\"data\":\"%@\"}", IOS_BUY_BACK, data];
        NSLog(@"return:%@", content);
        NSLog(@"unityObject:%@", self.unityObject);
        NSLog(@"unityMethod:%@", self.unityMethod);
        UnitySendMessage([self.unityObject UTF8String], [self.unityMethod UTF8String], [content UTF8String]);
    }
}


@end
