using System.Collections.Generic;

namespace LaClient
{
  public class LaClientModel
  {
    public int? TargetHp;
    public List<LaClientMember> Party = new List<LaClientMember>();
  }

  public class LaClientMember
  {
    public int? Hp;
    public int? Mp;
  }
}