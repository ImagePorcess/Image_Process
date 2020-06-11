using DMC;
using STOM.API;
using STOM.Model;
using STOM.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace STOM.Control
{
    class MotionControl
    {

        public static AccessDBService motionDB = new AccessDBService();

        public static DataTable motionAxisData = new DataTable();

        public static DataTable motionIOData = new DataTable();

        public static List<Model.MotionAxis> axisList = new List<Model.MotionAxis>();

        public static List<Model.MotionIO> ioList = new List<Model.MotionIO>();

        /// <summary>
        /// 初始化板卡
        /// </summary>
        public static void InitMotionCard()
        {
            try
            {
                ushort cardNum = DMC.DMC2210.d2210_board_init();

                if (cardNum == 0)
                {
                    System.Windows.Forms.MessageBox.Show("未找到运动控制卡");
                }
                else
                {
                    try
                    {
                        motionIOData = motionDB.QueryTable("MotionIO");

                        foreach (DataRow row in motionIOData.Rows)
                        {
                            Model.MotionIO io = new Model.MotionIO();
                            io.IoDescript = row["Descript"].ToString();
                            io.IoPointNum = Convert.ToUInt16(row["Num"]);
                            io.IoStatus = Convert.ToBoolean(row["Status"]);
                            io.IoStyle = Convert.ToBoolean(row["Style"]);
                            ioList.Add(io);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("初始化MotionIO失败" + ex.Message);
                    }

                    try
                    {
                        motionAxisData = motionDB.QueryTable("MotionAxis");
                        foreach (DataRow row in motionAxisData.Rows)
                        {
                            Model.MotionAxis axis = new Model.MotionAxis();
                            axis.ALMStatus = Convert.ToBoolean(row["ALMStatus"]);
                            axis.AxisEnable = Convert.ToBoolean(row["EnableStatus"]);
                            axis.CurPosition = Convert.ToDouble(row["CurPosition"]);
                            axis.Descript = row["Descript"].ToString();
                            axis.EmgEnable = Convert.ToBoolean(row["EmgEnable"]);
                            axis.EmgStatus = Convert.ToBoolean(row["EmgStatus"]);
                            axis.HomeDirection = Convert.ToBoolean(row["HomeDirection"]);
                            axis.HomeModel = Convert.ToBoolean(row["HomeModel"]);
                            axis.Millimeters = Convert.ToInt32(row["Millimeters"]);
                            axis.MaxSpeed = Convert.ToDouble(row["MaxSpeed"]);
                            axis.MinSpeed = Convert.ToDouble(row["MinSpeed"]);
                            axis.MoveEnable = Convert.ToBoolean(row["MoveEnable"]);
                            axis.MoveLength = Convert.ToDouble(row["MoveLength"]);
                            axis.MoveModel = Convert.ToInt32(row["MoveModel"]);
                            axis.Num = Convert.ToUInt16(row["Num"]);
                            axis.NegativeEL = Convert.ToBoolean(row["NegativeEL"]);
                            axis.Pluse = Convert.ToDouble(row["Pluse"]);
                            axis.PluseOutModel = Convert.ToInt32(row["PluseOutModel"]);
                            axis.PositiveEL = Convert.ToBoolean(row["PositiveEL"]);
                            axis.PowerEffective = Convert.ToBoolean(row["PowerEffective"]);
                            axisList.Add(axis);
                        }

                        SetAxisConfig();

                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("初始化MotionAxis失败" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("初始化板卡出现异常" + ex.Message);
            }

            
        }

        /// <summary>
        /// 设置所有轴的参数
        /// </summary>
        public static void SetAxisConfig()
        {
            foreach( MotionAxis axis in axisList)
            {
                DMC2210.d2210_set_pulse_outmode(Convert.ToUInt16(axis.Num), Convert.ToUInt16(axis.PluseOutModel));     //设置脉冲输出模式
                DMC2210.d2210_write_SEVON_PIN(Convert.ToUInt16(axis.Num), Convert.ToUInt16(axis.AxisEnable) );         //输出对指定轴的伺服使能端子的控制
                DMC2210.d2210_config_EMG_PIN(Convert.ToUInt16(axis.Num), Convert.ToUInt16(axis.EmgEnable), Convert.ToUInt16(axis.PowerEffective)); //EMG 使能开关,设置有效电平
                DMC2210.d2210_set_profile(Convert.ToUInt16(axis.Num), ConvertToPulse(axis, axis.MinSpeed), ConvertToPulse(axis,axis.MaxSpeed), 0.1, 0.1);
                DMC2210.d2210_config_EL_MODE(Convert.ToUInt16(axis.Num), 0);  
                DMC2210.d2210_counter_config(Convert.ToUInt16(axis.Num), 3);     //设置编码器输入口的计数方式为4倍AB相,AB相EA,EB
                DMC2210.d2210_config_EZ_PIN(Convert.ToUInt16(axis.Num), 0, 0);   //设置编码器的EZ相信号无效
                DMC2210.d2210_config_LTC_PIN(Convert.ToUInt16(axis.Num), 0, 1);  //设置指定轴锁存信号低电平有效
                DMC2210.d2210_config_latch_mode(Convert.ToUInt16(axis.Num), 0);  //设置LTC 端口为单轴锁存方式
                DMC2210.d2210_config_SD_PIN(Convert.ToUInt16(axis.Num), 0,  0 , 0); //不用减速信号
                DMC2210.d2210_config_ALM_PIN(Convert.ToUInt16(axis.Num), 0,  0);    //设置ALM 的低电平有效立即停
                DMC2210.d2210_config_PCS_PIN(Convert.ToUInt16(axis.Num),  0, 0);    //不适用PCS功能,PCS是用来终止当前运动执行新的运动指令
                DMC2210.d2210_config_INP_PIN(Convert.ToUInt16(axis.Num), 1, 0);     //设置允许INP信号（伺服电机到位信号）及其有效的逻辑电平为低电平有效
                DMC2210.d2210_config_ERC_PIN(Convert.ToUInt16(axis.Num), 1, 1, 0, 1); //设置允许/禁止ERC 信号及其有效电平和输出方式
            }           
        }
        /// <summary>
        /// 回原运动
        /// </summary>
        /// <param name="axis"></param>
        public static void AxisGoHome(Model.MotionAxis axis)
        {
            GetAxisStatus(axis);
            if (!axis.ALMStatus)
            {
                ushort axisNumber = Convert.ToUInt16(axis.Num);
                DMC2210.d2210_set_profile(axisNumber, ConvertToPulse(axis, axis.MinSpeed), ConvertToPulse(axis,axis.MaxSpeed), 0.1, 0.1);
                if (axis.OriStatus) return;                                //已经在原点了则返回
                DMC2210.d2210_set_HOME_pin_logic(axisNumber, 0, 1);        //原点信号的有效逻辑电平
                DMC2210.d2210_config_home_mode(axisNumber, 0, 0);          //设定回原点模式不计EZ信号
                DMC2210.d2210_home_move(axisNumber, Convert.ToUInt16(axis.HomeDirection), 0);  //按指定的方向和速度方式低速回原点。
                while (DMC2210.d2210_check_done(axisNumber) == 0) { Thread.Sleep(1); }           //等待回原点动作完成
                DMC2210.d2210_set_position(axisNumber, 0);                      //设置轴的指令脉冲计数器绝对位置为0
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(string.Format("{0}轴驱动器报警,请断电重启",axis.Num));
            }
        }

        /// <summary>
        /// 定长运动
        /// </summary>
        /// <returns>返回编码器脉冲值</returns>
        public static void PMove(Model.MotionAxis axis)
        {
            if ( axis.MoveEnable )
            {
                           //移动类型 S
                DMC2210.d2210_set_s_profile(Convert.ToUInt16(axis.Num), ConvertToPulse(axis, axis.MinSpeed), ConvertToPulse(axis,axis.MaxSpeed), 0.2, 0.2, 2000, 2000);
                DMC2210.d2210_s_pmove(Convert.ToUInt16(axis.Num), Convert.ToInt32(ConvertToPulse(axis,axis.MoveLength)), Convert.ToUInt16(axis.MoveModel));
                while (DMC2210.d2210_check_done(Convert.ToUInt16(axis.Num)) == 0) { System.Threading.Thread.Sleep(10); }
            }
        }

        /// <summary>
        /// 连续运动
        /// </summary>
        /// <param name="axis">运动轴</param>
        /// <param name="orientation">运动方向</param>
        public static void VMove(Model.MotionAxis axis, int orientation)
        {

            DMC2210.d2210_set_s_profile(Convert.ToUInt16(axis.Num), ConvertToPulse(axis, axis.MinSpeed), ConvertToPulse(axis, axis.MaxSpeed), 0.2, 0.2, 2000, 2000);
            DMC2210.d2210_s_vmove(Convert.ToUInt16(axis.Num), Convert.ToUInt16(orientation));
            
        }

        /// <summary>
        /// 减速停止指定轴
        /// </summary>
        /// <param name="axis"></param>
        public static void StopDecel(Model.MotionAxis axis)
        {
            DMC2210.d2210_decel_stop(Convert.ToUInt16(axis.Num), 0.1);
        }

        /// <summary>
        /// 紧急停止所有轴
        /// </summary>
        public static void Stop()
        {
            DMC2210.d2210_emg_stop();
        }

        /// <summary>
        /// 获取当前IO的状态信息
        /// </summary>
        /// <param name="io"></param>
        /// <returns></returns>
        public static int GetIOStatus(MotionIO io)
        {
            int status = 0;
            if( io.IoStyle )
            {
                status = DMC2210.d2210_read_inbit(0, io.IoPointNum);
            }
            else
            {
                status = DMC2210.d2210_read_outbit(0, io.IoPointNum);
            }
            return status;
        }

        /// <summary>
        /// 获取当前轴的状态信息和当前的位置
        /// </summary>
        /// <param name="axis"></param>
        public static void GetAxisStatus(MotionAxis axis)
        {
            ushort status = DMC2210.d2210_axis_io_status(Convert.ToUInt16(axisList.IndexOf(axis)));
            axis.ALMStatus = Convert.ToBoolean( status << 10 );
            axis.PositiveEL = Convert.ToBoolean(status << 12);
            axis.NegativeEL = Convert.ToBoolean(status << 13);
            axis.OriStatus = Convert.ToBoolean(status << 14);
            axis.CurPosition = ConvertPulseToPosition(axis, DMC2210.d2210_get_position(Convert.ToUInt16(axis.Num)));

        }

        /// <summary>
        /// 距离转脉冲
        /// </summary>
        /// <param name="values">输入速度mm/s 或者是长度 mm</param>
        /// <returns>返回的是 pulse/s 或者 pulse </returns>
        private static double ConvertToPulse(Model.MotionAxis axis ,double values)
        {
            double pulse = 0.0;

            pulse = values * (axis.Pluse / axis.Millimeters);

            return pulse;
        }

        /// <summary>
        /// 脉冲转距离
        /// </summary>
        /// <param name="values">输入脉冲 pulse</param>
        /// <returns>返回的是 mm </returns>
        private static double ConvertPulseToPosition(Model.MotionAxis axis, int values)
        {
            double position = 0.0;

            position = values * (axis.Millimeters / axis.Pluse);

            return position;
        }

    }
}
