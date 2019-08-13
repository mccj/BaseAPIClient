using System.Text;

namespace System.Linq
{
    /// <summary>
    /// 异常操作扩展
    /// </summary>
    internal static class ExceptionExtensions
    {
        /// <summary>
        /// 显示详细错误性息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        internal static string DetailMessage1(this Exception ex)
        {
            var eee = ex;
            string err = "";
            do
            {
                err += eee.Message + "\r\n";
                eee = eee.InnerException;
            } while (eee != null);
            return err;
        }
        ///// <summary>
        ///// 格式化异常消息
        ///// </summary>
        ///// <param name="e">异常对象</param>
        ///// <param name="isHideStackTrace">是否隐藏异常规模信息</param>
        ///// <returns>格式化后的异常信息字符串</returns>
        //internal static string FormatMessage(this Exception e, bool isHideStackTrace = false)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    int count = 0;
        //    string appString = string.Empty;
        //    while (e != null)
        //    {
        //        if (count > 0)
        //        {
        //            appString += "  ";
        //        }
        //        sb.AppendLine(string.Format("{0}异常消息：{1}", appString, e.Message));
        //        sb.AppendLine(string.Format("{0}异常类型：{1}", appString, e.GetType().FullName));
        //        sb.AppendLine(string.Format("{0}异常方法：{1}", appString, (e.TargetSite == null ? null : e.TargetSite.Name)));
        //        sb.AppendLine(string.Format("{0}异常源：{1}", appString, e.Source));
        //        if (!isHideStackTrace && e.StackTrace != null)
        //        {
        //            sb.AppendLine(string.Format("{0}异常堆栈：{1}", appString, e.StackTrace));
        //        }
        //        if (e.InnerException != null)
        //        {
        //            sb.AppendLine(string.Format("{0}内部异常：", appString));
        //            count++;
        //        }
        //        e = e.InnerException;
        //    }
        //    return sb.ToString();
        //}


        //internal static bool IsCatchableExceptionType(this Exception e)
        //{
        //    // a 'catchable' exception is defined by what it is not.
        //    var type = e.GetType();

        //    return ((type != typeof(StackOverflowException)) &&
        //            (type != typeof(OutOfMemoryException)) &&
        //            (type != typeof(Threading.ThreadAbortException)) &&
        //            (type != typeof(NullReferenceException)) &&
        //            (type != typeof(AccessViolationException)) &&
        //            !typeof(Security.SecurityException).IsAssignableFrom(type));
        //}

        //public static bool IsCatchableEntityExceptionType(this Exception e)
        //{
        //    DebugCheck.NotNull(e);

        //    var type = e.GetType();

        //    return IsCatchableExceptionType(e) &&
        //           type != typeof(Data.Entity.Core.EntityCommandExecutionException) &&
        //           type != typeof(Data.Entity.Core.EntityCommandCompilationException) &&
        //           type != typeof(Data.Entity.Core.EntitySqlException);
        //}

        //// <summary>
        //// Determines whether the given exception requires additional context from the update pipeline (in other
        //// words, whether the exception should be wrapped in an UpdateException).
        //// </summary>
        //// <param name="e"> Exception to test. </param>
        //// <returns> true if exception should be wrapped; false otherwise </returns>
        //public static bool RequiresContext(this Exception e)
        //{
        //    // if the exception isn't catchable, never wrap
        //    if (!e.IsCatchableExceptionType())
        //    {
        //        return false;
        //    }

        //    // update and incompatible provider exceptions already contain the necessary context
        //    return !(e is Data.Entity.Core.UpdateException) && !(e is Data.Entity.Core.ProviderIncompatibleException);
        //}
    }
}
