namespace DMC.DAL.Script
{
    public interface IRunRecord
    {
        /// <summary>
        /// �K�[
        /// <summary>
        string Addt_sRunRecord { get; }
        /// <summary>
        /// �ק�
        /// <summary>
        string Modt_sRunRecord { get; }
        /// <summary>
        /// ?��
        /// <summary>
        string Delt_sRunRecord { get; }
        /// <summary>
        /// ??�O�_�s�b
        /// <summary>
        string IsExitt_sRunRecord { get; }
        /// <summary>
        /// ���u�d?��^?�u��
        /// <summary>
        string GetListt_sRunRecord { get; }
        /// <summary>
        /// ���o�Ҧ�?�ƦC��
        /// <summary>
        string GetAllt_sRunRecord { get; }

        /// <summary>
        /// �K�[�u�@��x
        /// </summary>
        string AddJobRecord { get; }
        /// <summary>
        /// 
        /// </summary>
        string UpdateJobRecord { get; }
    }
}
