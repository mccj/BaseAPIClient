//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Utilities
//{
//    //public class 执行速度统计
//    //{
//    //    private readonly System.Collections.Concurrent.ConcurrentDictionary<string, long> pairs = new System.Collections.Concurrent.ConcurrentDictionary<string, long>();
//    //    public void 添加记录(int 数量 = 1)
//    //    {
//    //        var date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");
//    //        pairs.AddOrUpdate(date, 0, (f1, f2) => f2 + 数量);
//    //        var removeKeys = pairs.OrderByDescending(f => f.Key).Skip(保存记录).Select(f => f.Key).ToArray();
//    //        foreach (var item in removeKeys)
//    //        {
//    //            pairs.TryRemove(item, out long value);
//    //        }
//    //    }
//    //    public int 保存记录 { get; set; } = 1000;
//    //    public Dictionary<string, long> 获取历史记录(int count = 0)
//    //    {
//    //        var rr = pairs.OrderByDescending(f => f.Key);
//    //        if (count == 0)
//    //            return rr.ToDictionary(f => f.Key, f => f.Value);
//    //        else
//    //            return rr.Take(count).ToDictionary(f => f.Key, f => f.Value);
//    //    }
//    //}
//}
