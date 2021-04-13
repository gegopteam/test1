using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;


[ProtoContract]
public class PB_PhoneLoginAccount // 119
{
    [ProtoMember(1)]
    public int result;
    [ProtoMember(2)]
    public int accountType;
    [ProtoMember(3)]
    public int userId;
    [ProtoMember(4)]
    public string accountName;
    [ProtoMember(5)]
    public string strToken;
    [ProtoMember(6)]
    public string nickname;
}

[ProtoContract]
public class PBMsg_SetPhoneLoginPass // 120
{
    [ProtoMember(1)]
    public string phoneNumber;
    [ProtoMember(2)]
    public string password;
    [ProtoMember(3)]
    public string nickname;
}
[ProtoContract]
public class PBMsg_GetUserNicknameRequest // 121
{
    [ProtoMember(1)]
    public int languageType;
    [ProtoMember(2)]
    public List<string> nicknames = new List<string>();
}
