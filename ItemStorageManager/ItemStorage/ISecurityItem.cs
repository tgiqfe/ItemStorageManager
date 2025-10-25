using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ItemStorageManager.ItemStorage
{
    internal interface ISecurityItem
    {
        bool Grant(string account, string rights, string accessType, string inheritance, string propagation);
        bool Grant(string accessRuleText);
        bool Revoke(string account);
        bool RevokeAll();
        bool ChangeOwner(string newOwner);
        bool ChangeInherited(bool isInherited, bool preserve);
    }
}
