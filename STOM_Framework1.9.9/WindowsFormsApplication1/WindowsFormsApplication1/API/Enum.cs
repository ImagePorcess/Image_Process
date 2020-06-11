using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STOM.API
{
    /// <summary>
    /// 测试模式,0 -生产模式; 1 - GRR模式; 2-工程模式; 3-运动测试模式; 4-相关性测试模式
    /// </summary>
    public enum TestModel
    {
        ProduteModel = 0,
        GrrModel,
        DebugModel,
        MotionTestModel,
        CorrelationModel
    };
    /// <summary>
    /// 测试方式,0 - 1次1遍,1 - 1次3遍, 2- 1次9遍
    /// </summary>
    public enum TestWay
    {
        OneByOne = 0,
        OneByThree,
        OneByNine
    };
    /// <summary>
    /// 激发扫描方式0-正向一次,1-正向一次反向一次
    /// </summary>
    public enum ScanWay
    {
        OneWay = 0,
        TwoWay
    };
    /// <summary>
    /// 客户类型,0-深圳富士康, 1-上海富士康, 2-世硕
    /// </summary>
    public enum Customer
    {
        FoxconnShenZhen = 0,
        FoxconnShangHai,
        PEGA,
    };
    /// <summary>
    /// 功率表COM口
    /// </summary>
    public enum PowerPort
    {
        COM1 = 0,
        COM2,
        COM3,
        COM4,
        COM5,
        COM6
    };

}
