//
//  UnityLoginAgent.m
//  Unity-iPhone
//
//  Created by tianGe on 2017/11/30.
//
//

#import <Foundation/Foundation.h>
#import "UUIDProvider.h"
#import <sys/utsname.h>

extern "C"
{
    
    
    const char*  GetLoginUUID()
    {
        NSString * uuid = UUIDProvider::GetUUID();
        NSLog(@"--UnityLoginAgent GetLoginUUID uid = %@" , uuid);
        if(nullptr == uuid)
            return "";
        
        return strdup( [uuid UTF8String] );
    }
    
    const char*  GetDeviceName()
    {
        try {
            struct utsname systemInfo;
            uname(&systemInfo);
            NSString *deviceModel = [NSString stringWithCString:systemInfo.machine encoding:NSUTF8StringEncoding];
            // NSLog( @"--deviceModel %s---" , [deviceModel UTF8String] );
            if ( deviceModel == nil ) {
                return "";
            }
            return strdup( [deviceModel UTF8String] );
        } catch (NSException* exp ){
            
        }
       return "";
    }
    
    BOOL IsHasAccountLogin()
    {
        return UUIDProvider::GetAccountLogin();
    }
    void ToSetAccountLogin()
    {
        UUIDProvider::SetAccountLogin(@"1");
    }
    BOOL IsHasGuestLogin()
    {
        return UUIDProvider::GetGuestLogin();
    }
    void ToSetGuestLogin()
    {
        UUIDProvider::SetGuestLogin(@"1");
    }
    BOOL IsDownWX()
    {
        return UUIDProvider::IsDownLoadWX();
    }
}

extern "C"{

void GoToAppStore()
{
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@"itms-apps://itunes.apple.com/cn/app/%E6%96%B0%E9%BE%99%E6%8D%95%E9%B1%BC/id1272607635?mt=8"]];
}
    
}
