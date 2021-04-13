//
//  UUIDProvider.cpp
//  Unity-iPhone
//
//  Created by tg on 2018/8/2.
//

#include "UUIDProvider.h"
#include "KeyChainStore.h"
#include "WXApi.h"
UUIDProvider * UUIDProvider::m_pObject = nullptr;

UUIDProvider * UUIDProvider::GetInstance()
{
    if(nullptr == m_pObject)
    {
        m_pObject = new UUIDProvider();
    }
    return m_pObject;
}
void UUIDProvider::DestroyInstance()
{
    if(nullptr != m_pObject)
    {
        delete m_pObject;
        m_pObject = nullptr;
    }
}

UUIDProvider::UUIDProvider()
{
    
}

UUIDProvider::~UUIDProvider()
{
    
}

NSString * UUIDProvider::GetUUID()
{
    
    NSString * strUUID = (NSString *)[KeyChainStore load:@"com.xinlongbuyu.fishing3d.openid"];
    
    NSLog(@"UUIDProvider  strUUID 1 =%@",strUUID);
    
    //首次执行该方法时，uuid为空
    
    if ([strUUID isEqualToString:@""] || !strUUID)
        
    {
        
        //生成一个uuid的方法
        
        CFUUIDRef uuidRef = CFUUIDCreate(kCFAllocatorDefault);
        
        
        
        strUUID = (NSString *)CFBridgingRelease(CFUUIDCreateString (kCFAllocatorDefault,uuidRef));
       
        NSLog(@"UUIDProvider  strUUID 2 =%@",strUUID);

        
        // NSLog( strUUID );
        //将该uuid保存到keychain
        
        [KeyChainStore save:@"com.xinlongbuyu.fishing3d.openid" data:strUUID];
        
        NSLog(@"UUIDProvider  strUUID 3 = %@",strUUID);

        
        return strUUID;
    }else
    {
        return @"";
       // return strUUID;
    }
}
//获取是否有正式账号登录  "0" 表示没有账号登录,"1"表示有账号登录
BOOL UUIDProvider::GetAccountLogin()
{
    NSString * account = (NSString*)[KeyChainStore load:@"com.xinlongbuyu.fishing3d.AccountLogin"];
    //判断accout中是否有数据
    if ([account isEqualToString:@"0"] || nil == account)
    {
        // NSLog( strUUID );
        //将该保存到keychain
        NSString * account = @"0";
        
        [KeyChainStore save:@"com.xinlongbuyu.fishing3d.AccountLogin" data:account];
        
        return FALSE;
    }
   // return FALSE;
    return TRUE;
}

//设置正式账号登录
void UUIDProvider::SetAccountLogin(NSString *account)
{
    [KeyChainStore save:@"com.xinlongbuyu.fishing3d.AccountLogin" data:account];
}

//获取是否有游客登录  "0" 表示没有游客登录,"1"表示有游客登录
BOOL UUIDProvider::GetGuestLogin()
{
    NSString * guest = (NSString*)[KeyChainStore load:@"com.xinlongbuyu.fishing3d.GuestLogin"];

    if ([guest isEqualToString:@"0"] || nil == guest)
    {
        //将该保存到keychain
        NSString * guest = @"0";

        [KeyChainStore save:@"com.xinlongbuyu.fishing3d.GuestLogin" data:guest];

        return FALSE;
    }
 //   return FALSE;
    return TRUE;
}
//设置游客登录
void UUIDProvider::SetGuestLogin(NSString *guest)
{
    [KeyChainStore save:@"com.xinlongbuyu.fishing3d.GuestLogin" data:guest];
}
//是否安装微信
BOOL UUIDProvider::IsDownLoadWX()
{
    if ([WXApi isWXAppInstalled]) {
        return  TRUE;
    }else{
        return FALSE;
    }
}

