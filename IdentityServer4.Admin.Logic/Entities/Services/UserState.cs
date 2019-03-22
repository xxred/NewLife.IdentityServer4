





using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.Admin.Logic.Entities.Services
{
  public class UserState
  {
    public UserState(bool active, bool blocked, bool deleted)
    {
      this.Active = active;
      this.Blocked = blocked;
      this.Deleted = deleted;
    }

    public UserState(string concatenatedStates)
    {
      if (string.IsNullOrWhiteSpace(concatenatedStates))
        return;
      string[] strArray = concatenatedStates.Split(',');
      this.Active = ((IEnumerable<string>) strArray).Any<string>((Func<string, bool>) (x => string.Compare("active", x, StringComparison.CurrentCultureIgnoreCase) == 0));
      this.Blocked = ((IEnumerable<string>) strArray).Any<string>((Func<string, bool>) (x => string.Compare("blocked", x, StringComparison.CurrentCultureIgnoreCase) == 0));
      this.Deleted = ((IEnumerable<string>) strArray).Any<string>((Func<string, bool>) (x => string.Compare("deleted", x, StringComparison.CurrentCultureIgnoreCase) == 0));
    }

    public bool Active { get; }

    public bool Blocked { get; }

    public bool Deleted { get; }

    public override bool Equals(object obj)
    {
      UserState userState;
      if (obj == null || (userState = obj as UserState) == null)
        return false;
      return this.Active == userState.Active && this.Blocked == userState.Blocked && this.Deleted == userState.Deleted;
    }

    public override int GetHashCode()
    {
      bool flag = this.Active;
      int hashCode1 = flag.GetHashCode();
      flag = this.Blocked;
      int hashCode2 = flag.GetHashCode();
      int num = hashCode1 + hashCode2;
      flag = this.Deleted;
      int hashCode3 = flag.GetHashCode();
      return num + hashCode3;
    }
  }
}
