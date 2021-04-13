//
//  UUIDProvider.h
//  Unity-iPhone
//
//  Created by tg on 2018/8/2.
//

#ifndef UUIDProvider_h
#define UUIDProvider_h

#include <stdio.h>

class UUIDProvider
{
public:
    static UUIDProvider * GetInstance();
    static void DestroyInstance();
private:
    static UUIDProvider *m_pObject;
    
private:
    UUIDProvider();
    ~UUIDProvider();
    
public:
    static NSString * GetUUID();
    static BOOL GetAccountLogin();
    static void SetAccountLogin(NSString *account);
    static BOOL GetGuestLogin();
    static void SetGuestLogin(NSString *guest);
    static BOOL IsDownLoadWX();
};

#endif /* UUIDProvider_h */
