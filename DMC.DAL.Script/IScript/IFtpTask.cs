namespace DMC.DAL.Script
{
    public interface IFtpTask
    {
        /// <summary>
        /// �K�[
        /// <summary>
        string Addt_sFtpTask { get; }
        /// <summary>
        /// �ק�
        /// <summary>
        string Modt_sFtpTask { get; }
        /// <summary>
        /// ?��
        /// <summary>
        string Delt_sFtpTask { get; }
        string DelTaskIDList { get; }
        /// <summary>
        /// ??�O�_�s�b
        /// <summary>
        string IsExitt_sFtpTask { get; }
        /// <summary>
        /// ���u�d?��^?�u��
        /// <summary>
        string GetListt_sFtpTask { get; }
        /// <summary>
        /// ���o�Ҧ�?�ƦC��
        /// <summary>
        string GetAllt_sFtpTask { get; }
        /// <summary>
        /// ����u�@������FTP�B�J
        /// </summary>
        string GetFtpSteps { get; }
    }
}
