using APM.Core.Models.Interfaces;

namespace APM.Core.Models;

public class GroupModel(int id, int pid, string name) : IGroup
{
    public int Id { get; set; } = id;
    public int Pid { get; set; } = pid;
    public string Title { get; set; } = name;

    public override string ToString()
    {
        return Title;
    }
}