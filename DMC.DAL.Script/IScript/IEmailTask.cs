namespace DMC.DAL.Script
{
    public interface IEmailTask
    {
        /// <summary>
        /// �K�[
        /// <summary>
        string Addt_sEmailTask { get; }
        /// <summary>
        /// �ק�
        /// <summary>
        string Modt_sEmailTask { get; }
        /// <summary>
        /// ?��
        /// <summary>
        string Delt_sEmailTask { get; }
        string DelTaskIDList { get; }
        /// <summary>
        /// ??�O�_�s�b
        /// <summary>
        string IsExitt_sEmailTask { get; }
        /// <summary>
        /// ���u�d?��^?�u��
        /// <summary>
        string GetListt_sEmailTask { get; }
        /// <summary>
        /// ���o�Ҧ�?�ƦC��
        /// <summary>
        string GetAllt_sEmailTask { get; }

        /// <summary>
        /// ����u�@�������l��B�J
        /// </summary>
        string GetEmailSteps { get; }
    }
}
