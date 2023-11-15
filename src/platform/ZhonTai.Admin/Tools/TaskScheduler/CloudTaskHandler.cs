using FreeSql;

namespace ZhonTai.Admin.Tools.TaskScheduler;

public class CloudTaskHandler : TaskHandler
{
    public CloudTaskHandler(FreeSqlCloud fsqlc, string dbKey): base(fsqlc.Use(dbKey))
    {
    }
}
