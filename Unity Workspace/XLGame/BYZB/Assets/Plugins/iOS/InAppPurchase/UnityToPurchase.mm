
#import "PurchaseIOS.h"

extern "C"
{
    void InitPurchaseIOS()
    {
        [PurchaseIOS getInstance];
        return;
    }
    
    void BuyProductIOS(char *productID)
    {
        //NSString *nsProductID = [[NSString alloc] init];
        NSString *nsProductID = [NSString stringWithUTF8String:productID];
        //nsProductID = [NSString stringWithUTF8String:productID];
        //nsProductID = [NSString stringWithCString:productID encoding:NSUTF8StringEncoding];
        NSLog(@"iOS Purchase:%@", nsProductID);
        [[PurchaseIOS getInstance] payProductWithID:nsProductID];
        return;
    }
    
    void SetPurchaseBackIOS(char *obj, char *method)
    {
        PurchaseIOS *purchaseIOS = [PurchaseIOS getInstance];
        if(nil!=purchaseIOS)
        {
            purchaseIOS.unityObject = [NSString stringWithUTF8String:obj];
            purchaseIOS.unityMethod = [NSString stringWithUTF8String:method];
        }
        return;
    }
    
    void Fun(char *title, char *msg, char *url)
    {
        //*
        NSString* nsTitle = [[NSString alloc] initWithUTF8String:title];
        NSString* nsMsg = [[NSString alloc] initWithUTF8String:msg];
        NSString* nsUrl = [[NSString alloc] initWithUTF8String:url];
        
        UIAlertView* view = [[UIAlertView alloc] initWithTitle:nsTitle
                                                       message:nsMsg
                                                      delegate:nil
                                             cancelButtonTitle:NSLocalizedString(@"Close", nil)
                                             otherButtonTitles:nil];
        
        [view show];
        //*/
    }
}
