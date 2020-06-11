///DMC2210控制卡DLL非托管动态运行库(c#)
///适用于DMC2210
///创建时间 2015/08/14

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


namespace DMC
{
    public class DMC2210
    {

        #region   板卡初始和配置函数  
        /// <summary>
        /// 为DMC2210运动控制卡分配系统资源并初始化控制卡
        /// </summary>
        /// <returns>卡数，(0～8)，其中0表示没有卡</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_board_init", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt16 d2210_board_init();

       /// <summary>
       /// 释放控制卡占用的系统资源。当程序结束时必须调用此函数，它与d2210_board_init函数是一个相反的过程
       /// </summary>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_board_close", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_board_close();

        #endregion

        #region  脉冲输入输出配置
        
        /// <summary>
        /// 设置指定轴的脉冲输出模式
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="outmode">见手册说明表</param>
        /// 注意事项：在调用运动函数（如：d2210_t_vmove 等）输出脉冲之前，一定要根据驱动器接收
        /// 脉冲的模式调用d2210_set_pulse_outmode设置控制卡脉冲输出模式.
        [DllImport("DMC2210.dll", EntryPoint = "d2210_set_pulse_outmode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_set_pulse_outmode(UInt16 axis, UInt16 outmode);

        //专用信号设置函数
        /// <summary>
        /// 设置SD信号有效的逻辑电平及其工作方式
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="enable">允许/禁止信号功能：0－无效，1－有效</param>
        /// <param name="sd_logic">设置SD信号的有效逻辑电平：0－低电平有效，1－高电平有效</param>
        /// <param name="sd_mode">设置SD信号的工作方式：
        ///                         0－减速到起始速度，如果SD信号丢失，又开始加速
        ///                         1－减速到起始速度，并停止，如果在减速过程中，SD信号丢失，又开始加速
        ///                         2－锁存SD信号，并减速到起始速度
        ///                         3－锁存SD信号，并减速到起始速度后停止。
        ///</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_config_SD_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_config_SD_PIN(UInt16 axis, UInt16 enable, UInt16 sd_logic, UInt16 sd_mode);
        
        /// <summary>
        /// 设置允许/禁止PCS外部信号在运动中改变目标位置
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="enable">允许/禁止信号功能：0－无效，1－有效</param>
        /// <param name="pcs_logic">设置PCS信号的有效电平：0－低电平有效，1－高电平有效</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_config_PCS_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_config_PCS_PIN(UInt16 axis, UInt16 enable, UInt16 pcs_logic);
        
        /// <summary>
        /// 设置允许/禁止INP信号及其有效的逻辑电平
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="enable">允许/禁止信号功能：0－无效，1－有效</param>
        /// <param name="inp_logic">设置INP信号的有效电平：0－低电平有效，1－高电平有效</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_config_INP_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_config_INP_PIN(UInt16 axis, UInt16 enable, UInt16 inp_logic);
        
        /// <summary>
        /// 设置允许/禁止ERC信号及其有效电平和输出方式
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="enable">enable 范围：0～3：
        ///                         0－不自动输出ERC信号
        ///                         1－接收EL、ALM、CEMG等信号停止时，自动输出ERC信号
        ///                         2－接收ORG信号时，自动输出ERC信号
        ///                         3－满足第1或2两项条件时，均会自动输出ERC信号
        /// </param>
        /// <param name="erc_logic">设置ERC信号的有效电平：0－低电平有效，1－高电平有效</param>
        /// <param name="erc_width">误差清除信号ERC有效输出宽度：
        ///                             0－12us
        ///                             1－102us
        ///                             2－409us
        ///                             3－1.6ms
        ///                             4－13ms
        ///                             5－52ms
        ///                             6－104ms
        ///                             7－电平输出
        ///</param>
        /// <param name="erc_off_time">ERC信号的关断时间：0－0us，1－12us，2－1.6ms，3－104ms</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_config_ERC_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_config_ERC_PIN(UInt16 axis, UInt16 enable, UInt16 erc_logic, UInt16 erc_width, UInt16 erc_off_time);

        /// <summary>
        /// 设置ALM的逻辑电平及其工作方式
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="alm_logic">ALM信号的输入电平：0－低电平有效，1－高电平有效</param>
        /// <param name="alm_action">ALM信号的制动方式：0－立即停止，1－减速停止</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_config_ALM_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_config_ALM_PIN(UInt16 axis, UInt16 alm_logic, UInt16 alm_action);
        
        /// <summary>
        /// 设置EL信号的有效电平及制动方式
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="el_mode">EL有效电平和制动方式：
        ///                         0－立即停、低有效
        ///                         1－减速停、低有效
        ///                         2－立即停、高有效
        ///                         3－减速停、高有效</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_config_EL_MODE", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_config_EL_MODE(UInt16 axis, UInt16 el_mode);
        
        /// <summary>
        /// 设置ORG信号的有效电平，以及允许/禁止滤波功能
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="org_logic">ORG信号的有效电平：0－低电平有效，1－高电平有效</param>
        /// <param name="filter">允许/禁止滤波功能：0－禁止，1－允许</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_set_HOME_pin_logic", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_set_HOME_pin_logic(UInt16 axis, UInt16 org_logic, UInt16 filter);

        /// <summary>
        /// 输出对指定轴的伺服使能端子的控制
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="on_off">设定管脚电平状态：0－低，1－高。SEVON输出口初始状态可选</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_write_SEVON_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_write_SEVON_PIN(UInt16 axis, UInt16 on_off);
        
        /// <summary>
        /// 读取指定轴的“伺服使能”端子的电平状态
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>0－低电平，1－高电平。SEVON输出口初始状态可选</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_read_SEVON_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2210_read_SEVON_PIN(UInt16 axis);

        /// <summary>
        /// 控制指定轴“误差清除”端子信号的输出
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="sel">0－复位ERC信号，1－输出ERC信号</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_write_ERC_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_write_ERC_PIN(UInt16 axis, UInt16 sel);
        
        /// <summary>
        /// 读取指定运动轴的“伺服准备好”端子的电平状态
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>0－低电平，1－高电平</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_read_RDY_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2210_read_RDY_PIN(UInt16 axis);


        /// <summary>
        /// EMG信号设置，急停信号有效后会立即停止所有轴
        /// </summary>
        /// <param name="cardno">卡号</param>
        /// <param name="enable">0：无效 ; 1：有效</param>
        /// <param name="emg_logic">0：:低有效 ; 1：高有效</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_config_EMG_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_config_EMG_PIN(UInt16 cardno, UInt16 enable, UInt16 emg_logic);

        #endregion

        #region 通用输入/输出控制函数

        /// <summary>
        /// 读取指定控制卡的某一位输入口的电平状态
        /// </summary>
        /// <param name="cardno">指定控制卡号, 范围（0～N – 1，N为卡数）</param>
        /// <param name="bitno">指定输入口位号（取值范围：1～20）</param>
        /// <returns>0表示低电平；1表示高电平</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_read_inbit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2210_read_inbit(UInt16 cardno, UInt16 bitno);
        
        /// <summary>
        /// 对指定控制卡的某一位输出口置位
        /// </summary>
        /// <param name="cardno">指定控制卡号，范围（0～N – 1，N为卡数）</param>
        /// <param name="bitno">指定输出口位号（取值范围：1～20）</param>
        /// <param name="on_off">输出电平：0－表示输出低电平
        ///                                1－表示输出高电平
        ///                        当拨码开关S1对应的位设置为OFF时，对输出口1～12置位时，0表
        ///                         示高电平；1表示低电平
        ///</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_write_outbit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_write_outbit(UInt16 cardno, UInt16 bitno, UInt16 on_off);
        
        /// <summary>
        /// 读取指定控制卡的某一位输出口的电平状态
        /// </summary>
        /// <param name="cardno">指定控制卡号, 范围（0 － N - 1 ,N为卡数）</param>
        /// <param name="bitno">指定输入口位号（取值范围：1－20）</param>
        /// <returns>0表示低电平；1表示高电平。
        ///          当拨码开关S1对应的位设置为OFF时，读取输出口1 –12的状态时，0表示高电平；
        ///          1表示低电平
        ///</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_read_outbit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2210_read_outbit(UInt16 cardno, UInt16 bitno);

        /// <summary>
        /// 读取指定控制卡的全部通用输入口的电平状态
        /// </summary>
        /// <param name="cardno">指定控制卡号，范围（0～N – 1，N为卡数）</param>
        /// <returns>：bit0～bit19位值分别代表第1～20号输入端口值</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_read_inport", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2210_read_inport(UInt16 cardno);

        /// <summary>
        /// 读取指定控制卡的全部通用输出口的电平状态
        /// </summary>
        /// <param name="cardno">指定控制卡号，范围（0～N – 1，N为卡数）</param>
        /// <returns>bit0～bit19位值分别代表第1～20号输出端口值当拨码开关S1对应的位设置为OFF时，
        /// 读取输出口1 –12的状态时，0表示高电平；1表示低电平。
        /// </returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_read_outport", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2210_read_outport(UInt16 cardno);
        
        /// <summary>
        /// 指定控制卡的全部通用输出口的电平状态
        /// </summary>
        /// <param name="cardno">指定控制卡号，范围（0～N – 1，N为卡数）</param>
        /// <param name="port_value">bit0～bit19位值分别代表第1～20号输出端口值。
        ///当拨码开关S1对应的位设置为OFF时，对输出口1 –12置位时，0表示
        ///高电平；1表示低电平。
        ///</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_write_outport", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_write_outport(UInt16 cardno, UInt32 port_value);

        #endregion

        #region  制动函数

        /// <summary>
        /// 指定轴减速停止，调用此函数时立即减速，减速到起始速度后停止脉冲输出
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="Tdec">减速时间</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_decel_stop", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_decel_stop(UInt16 axis, double Tdec);
        
        /// <summary>
        /// 使指定轴立即停止，没有任何减速的过程
        /// </summary>
        /// <param name="axis">轴号</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_imd_stop", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_imd_stop(UInt16 axis);
        
        /// <summary>
        /// 使所有的运动轴紧急停止
        /// </summary>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_emg_stop", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_emg_stop();
        
        
        [DllImport("DMC2210.dll", EntryPoint = "d2210_simultaneous_stop", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_simultaneous_stop(UInt16 axis);

        //位置设置和读取函数
        /// <summary>
        /// 读取指定轴的指令脉冲位置
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>指定运动轴的命令脉冲数，单位：脉冲</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_get_position", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2210_get_position(UInt16 axis);
        
        /// <summary>
        /// 设置指定轴的指令脉冲位置
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="current_position">绝对位置值</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_set_position", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_set_position(UInt16 axis, Int32 current_position);

        #endregion

        #region  状态检测函数

        /// <summary>
        /// 检测指定轴的运动状态，停止或是在运行中
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>0表示指定轴正在运行，1表示指定轴已停止</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_check_done", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt16 d2210_check_done(UInt16 axis);
        
        /// <summary>
        /// 读取指定轴的预置缓冲区的状态
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>0－缓冲区空，1－缓冲区满</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_prebuff_status", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt16 d2210_prebuff_status(UInt16 axis);

        /// <summary>
        /// 读取指定轴有关运动信号的状态，包含指定轴的专用I/O状态。
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>放回值说明 请查阅手册</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_axis_io_status", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt16 d2210_axis_io_status(UInt16 axis);
        
        
        /// <summary>
        /// 读取指定轴的外部信号状态
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>放回值说明 请查阅手册</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_get_rsts", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2210_get_rsts(UInt16 axis);

        #endregion

        #region  速度设置

        /// <summary>
        /// 设定指定轴改变的速度上限，及变速使能
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="chg_enable">禁止/使能连续运行中变速（禁止保留）</param>
        /// <param name="Max_Vel">运行速度的变速上限值</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_variety_speed_range", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_variety_speed_range(UInt16 axis, UInt16 chg_enable, double Max_Vel);
        
        /// <summary>
        /// 读取当前速度值
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>轴的速度脉冲数</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_read_current_speed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern double d2210_read_current_speed(UInt16 axis);

        /// <summary>
        /// 在线改变指定轴的当前运动速度。该函数只适用于单轴运动中的变速，
        /// 且在调用前必须先调用d2210_variety_speed_range设置变速范围和使能
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="Curr_Vel">新的运行速度</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_change_speed", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_change_speed(UInt16 axis, double Curr_Vel);
        
        /// <summary>
        /// 设定插补矢量运动曲线的起始速度、运行速度、加速时间、减速时间
        /// </summary>
        /// <param name="Min_Vel">起始速度</param>
        /// <param name="Max_Vel">运行速</param>
        /// <param name="Tacc">总加速时间</param>
        /// <param name="Tdec">总减速时间</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_set_vector_profile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_set_vector_profile(double Min_Vel, double Max_Vel, double Tacc, double Tdec);
        
       
        /// <summary>
        /// 设定梯形曲线的起始速度、运行速度、加速时间、减速时间
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="Min_Vel">起始速度</param>
        /// <param name="Max_Vel">运行速度</param>
        /// <param name="Tacc">总加速时间</param>
        /// <param name="Tdec">总减速时间</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_set_profile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_set_profile(UInt16 axis, double Min_Vel, double Max_Vel, double Tacc, double Tdec);
        
        /// <summary>
        /// 设定S形曲线运动的起始速度、运行速度、总加减速时间、S段加减速脉冲数
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="Min_Vel">起始速度</param>
        /// <param name="Max_Vel">运行速度</param>
        /// <param name="Tacc">总加速时间</param>
        /// <param name="Tdec">总减速时间</param>
        /// <param name="Sacc">S加速段脉冲数</param>
        /// <param name="Sdec">S减速段脉冲数</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_set_s_profile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_set_s_profile(UInt16 axis, double Min_Vel, double Max_Vel, double Tacc, double Tdec, Int32 Sacc, Int32 Sdec);
        
        /// <summary>
        /// 设定S形曲线运动的起始速度、运行速度、总加减速时间、S段加减速时间
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="Min_Vel">起始速度</param>
        /// <param name="Max_Vel">运行速度</param>
        /// <param name="Tacc">总加速时间</param>
        /// <param name="Tdec">总减速时间</param>
        /// <param name="Tsacc">S加速段时间，其值应小于Tacc的一半</param>
        /// <param name="Tsdec">S减速段时间，其值应小于Tdec的一半</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_set_st_profile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_set_st_profile(UInt16 axis, double Min_Vel, double Max_Vel, double Tacc, double Tdec, double Tsacc, double Tsdec);
        
        /// <summary>
        /// 在相对模式的单轴定长运动中改变目标位置
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="dist">相对位置值</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_reset_target_position", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_reset_target_position(UInt16 axis, Int32 dist);
        
        #endregion

        #region 单轴定长运动

        /// <summary>
        /// 使指定轴以对称梯形速度曲线做定长位移运动
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="Dist">（绝对/相对）位移值，单位：脉冲数</param>
        /// <param name="posi_mode">位移模式设定：0表示相对位移，1表示绝对位移</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_t_pmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_t_pmove(UInt16 axis, Int32 Dist, UInt16 posi_mode);
        
        /// <summary>
        /// 使指定轴以非对称梯形速度曲线做定长位移运动
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="Dist">（绝对/相对）位移值，单位：脉冲数</param>
        /// <param name="posi_mode">位移模式设定：0表示相对位移，1表示绝对位移</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_ex_t_pmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_ex_t_pmove(UInt16 axis, Int32 Dist, UInt16 posi_mode);

        /// <summary>
        /// 使指定轴以对称S形速度曲线做定长位移运动
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="Dist">（绝对/相对）位移值，单位：脉冲数</param>
        /// <param name="posi_mode">位移模式设定：0表示相对位移，1表示绝对位移</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_s_pmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_s_pmove(UInt16 axis, Int32 Dist, UInt16 posi_mode);
        
        /// <summary>
        /// 使指定轴以非对称S形速度曲线做定长位移运动
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="Dist">（绝对/相对）位移值，单位：脉冲数</param>
        /// <param name="posi_mode">位移模式设定：0表示相对位移，1表示绝对位移</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_ex_s_pmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_ex_s_pmove(UInt16 axis, Int32 Dist, UInt16 posi_mode);

        #endregion

        #region 单轴连续运动

        /// <summary>
        /// 使指定轴以S形速度曲线加速到高速，并持续运行下去
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="dir">指定运动的方向，其中0表示负方向，1表示正方向</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_s_vmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_s_vmove(UInt16 axis, UInt16 dir);
        
        /// <summary>
        /// 使指定轴以梯形速度曲线加速到高速，并持续运行下去
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="dir">指定运动的方向，其中0表示负方向，1表示正方向</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_t_vmove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_t_vmove(UInt16 axis, UInt16 dir);

        #endregion

        #region 线性插补

        /// <summary>
        /// 指定任意两轴以对称的梯形速度曲线做插补运动
        /// </summary>
        /// <param name="axis1">指定两轴插补的第一轴</param>
        /// <param name="Dist1">指定两轴插补的第二轴</param>
        /// <param name="axis2">指定axis1的位移值</param>
        /// <param name="Dist2">指定axis2的位移值</param>
        /// <param name="posi_mode">位移模式设定：0表示相对位移，1表示绝对位移</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_t_line2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_t_line2(UInt16 axis1, Int32 Dist1, UInt16 axis2, Int32 Dist2, UInt16 posi_mode);

        #endregion

        #region 手轮运动

        /// <summary>
        /// 设置输入手轮脉冲信号的计数方式
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="inmode">设置脉冲输入方式：
        ///                             0－A、B相位信号，1倍计数
        ///                             1－A、B相位信号，2倍计数
        ///                             2－A、B相位信号，4倍计数
        ///                             3－差分脉冲信号
        ///</param>
        /// <param name="count_dir">设置计数器的计数方向：
        ///                                  0－默认的PA/PB输入计数方向
        ///                                  1－相反的PA/PB输入计数方向
        ///</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_set_handwheel_inmode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_set_handwheel_inmode(UInt16 axis, UInt16 inmode, UInt16 count_dir);
        
        /// <summary>
        /// 启动指定轴的手轮脉冲运动
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="vh">最大脉冲输入频率</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_handwheel_move", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_handwheel_move(UInt16 axis, double vh);

        #endregion

        #region  找原点

        /// <summary>
        /// 设定指定轴的回原点模式
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="mode">回原点的信号模式:0－只计home, 1－计home和EZ</param>
        /// <param name="EZ_count">遇到原点信号后，EZ信号出现EZ_count指定的次数后，轴运动停止。
        /// 仅当mode=1时该设置有效，取值范围：1－16
        /// </param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_config_home_mode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_config_home_mode(UInt16 axis, UInt16 mode, UInt16 EZ_count);
        
        /// <summary>
        /// 单轴回原点运动
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="home_mode">回原点方式：1－正方向回原点，2－负方向回原点</param>
        /// <param name="vel_mode">回原点速度：0－低速回原点，1－高速回原点</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_home_move", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_home_move(UInt16 axis, UInt16 home_mode, UInt16 vel_mode);
        #endregion

        #region 圆弧插补 

        /// <summary>
        /// 以当前位置为起点，按指定的圆心、目标绝对位置和方向作圆弧插补运动
        /// </summary>
        /// <param name="axis">轴号列表指针</param>
        /// <param name="target_pos">目标绝对位置列表指针</param>
        /// <param name="cen_pos">圆心绝对位置列表指针</param>
        /// <param name="arc_dir">圆弧方向：0表示顺时针，1表示逆时针</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_arc_move", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_arc_move(ref UInt16 axis, ref Int32 target_pos, ref Int32 cen_pos, UInt16 arc_dir);
        
        /// <summary>
        /// 以当前位置为起点，按指定的圆心、目标相对位置和方向作圆弧插补运动
        /// </summary>
        /// <param name="axis">轴号列表指针</param>
        /// <param name="rel_pos">目标相对位置列表指针</param>
        /// <param name="rel_cen">圆心相对位置列表指针</param>
        /// <param name="arc_dir">圆弧方向：0表示顺时针，1表示逆时针</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_rel_arc_move", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_rel_arc_move(ref UInt16 axis, ref Int32 rel_pos, ref Int32 rel_cen, UInt16 arc_dir);

        #endregion

        #region 设置和读取位置比较信号

        /// <summary>
        /// 配置位置比较输出端口的功能
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="cmp1_enable">配置CMP1(X轴)、CMP2(Y轴)
        ///                             0－配置为数字输出端口
        ///                             1－配置为位置比较输出
        ///</param>
        /// <param name="cmp2_enable">配置OUT17(X轴)、OUT18(Y轴)
        ///                             0－配置为数字输出端口
        ///                             1－配置为位置比较输出</param>
        /// <param name="CMP_logic">0－负逻辑；1－正逻辑</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_config_CMP_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_config_CMP_PIN(UInt16 axis, UInt16 cmp1_enable, UInt16 cmp2_enable, UInt16 CMP_logic);
        
        /// <summary>
        /// 读取指定轴的比较输出端口的电平
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>1－高电平；0－低电平</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_read_CMP_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 d2210_read_CMP_PIN(UInt16 axis);
        
        /// <summary>
        /// 设置指定轴的位置比较输出端口的电平
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="on_off">1－高电平；0－低电平</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_write_CMP_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_write_CMP_PIN(UInt16 axis, UInt16 on_off);
        
        /// <summary>
        /// 配置指定轴2个比较器的触发条件
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="cmp1_condition">比较器1的触发条件
        ///                     0：关闭比较器1功能
        ///                     1：计数器的值等于比较器1
        ///                     2：计数器的值小于比较器1
        ///                     3：计数器的值大于比较器1
        ///</param>
        /// <param name="cmp2_condition">比较器2的触发条件
        ///                     0：关闭比较器2功能
        ///                     1：计数器的值等于比较器2
        ///                     2：计数器的值小于比较器2
        ///                     3：计数器的值大于比较器2
        ///</param>
        /// <param name="source_sel">配置计数器类型
        ///             0：2个比较器均与指令脉冲计数器比较
        ///             1：比较器1和指令脉冲比较；比较器2与编码器脉冲比较</param>
        /// <param name="SL_action">0：保留</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_config_comparator", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_config_comparator(UInt16 axis, UInt16 cmp1_condition, UInt16 cmp2_condition, UInt16 source_sel, UInt16 SL_action);
       
        /// <summary>
        /// 预置比较器数值
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="cmp1_data">比较器1的值</param>
        /// <param name="cmp2_data">比较器2的值</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_set_comparator_data", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_set_comparator_data(UInt16 axis, UInt32 cmp1_data, UInt32 cmp2_data);

        #endregion

        #region 编码器计数功能PLD

        /// <summary>
        /// 读取指定轴编码器反馈位置脉冲计数值，范围：28位有符号数
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>位置反馈脉冲值</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_get_encoder", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2210_get_encoder(UInt16 axis);
        
        /// <summary>
        /// 设置指定轴编码器反馈脉冲计数值，范围：28位有符号数
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="encoder_value">编码器的设定值</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_set_encoder", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_set_encoder(UInt16 axis, UInt32 encoder_value);
        
        /// <summary>
        /// 设置指定轴的EZ信号的有效电平及其作用
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="ez_logic">EZ信号逻辑电平：0－低有效，1－高有效</param>
        /// <param name="ez_mode">EZ信号的工作方式：
        ///                             0－EZ信号无效
        ///                             1－EZ是计数器复位信号
        ///                             2－EZ是原点信号，且不复位计数器
        ///                             3－EZ是原点信号，且复位计数器
        ///</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_config_EZ_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_config_EZ_PIN(UInt16 axis, UInt16 ez_logic, UInt16 ez_mode);
        
        /// <summary>
        /// 设置指定轴锁存信号的有效电平
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="ltc_logic">LTC信号逻辑电平：0－低有效，1－高有效</param>
        /// <param name="ltc_mode">保留，可设为任意值</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_config_LTC_PIN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_config_LTC_PIN(UInt16 axis, UInt16 ltc_logic, UInt16 ltc_mode);
        
        /// <summary>
        /// 设置锁存方式为单轴锁存或是两轴同时锁存
        /// </summary>
        /// <param name="cardno">指定控制卡号</param>
        /// <param name="all_enable">锁存方式 ：0－单独锁存， 1－两轴同时锁存</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_config_latch_mode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_config_latch_mode(UInt16 cardno, UInt16 all_enable);

        /// <summary>
        /// 设置编码器的计数方式
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="mode">编码器器的计数方式：
        ///                         0 非A/B相 (脉冲/方向)
        ///                         1 1X A/B
        ///                         2 2X A/B
        ///                         3 4X A/B
        ///</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_counter_config", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_counter_config(UInt16 axis, UInt16 mode);
        
        /// <summary>
        /// 读取编码器锁存器的值
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>锁存器内的编码器脉冲数，单位：脉冲</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_get_latch_value", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2210_get_latch_value(UInt16 axis);
        
        /// <summary>
        /// 读取指定控制卡的锁存器的标志位
        /// </summary>
        /// <param name="cardno">指定控制卡号，范围（0～N – 1，N为卡数）</param>
        /// <returns>见相关手册表</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_get_latch_flag", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2210_get_latch_flag(UInt16 cardno);
        
        /// <summary>
        /// 复位指定控制卡的锁存器的标志位
        /// </summary>
        /// <param name="cardno">指定控制卡号</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_reset_latch_flag", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_reset_latch_flag(UInt16 cardno);
        
        /// <summary>
        /// 读取指定控制卡的计数器的标识位
        /// </summary>
        /// <param name="cardno">指定控制卡号</param>
        /// <returns>见相关手册表</returns>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_get_counter_flag", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 d2210_get_counter_flag(UInt16 cardno);
        
        /// <summary>
        /// 复位计数器的计数标志位, 范围（0～N – 1，N为卡数）
        /// </summary>
        /// <param name="cardno">指定控制卡号</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_reset_counter_flag", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_reset_counter_flag(UInt16 cardno);
        
        /// <summary>
        /// 复位计数器的清零标志位, 范围（0～N – 1，N为卡数）
        /// </summary>
        /// <param name="cardno">指定控制卡号</param>
        [DllImport("DMC2210.dll", EntryPoint = "d2210_reset_clear_flag", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_reset_clear_flag(UInt16 cardno);
        [DllImport("DMC2210.dll", EntryPoint = "d2210_triger_chunnel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_triger_chunnel(UInt16 cardno, UInt16 num);
        [DllImport("DMC2210.dll", EntryPoint = "d2210_set_speaker_logic", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void d2210_set_speaker_logic(UInt16 cardno, UInt16 logic);
        
        #endregion

      
    }
}
