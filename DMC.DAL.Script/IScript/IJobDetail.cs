namespace DMC.DAL.Script
{
    public interface IJobDetail
    {
        /// <summary>
        /// �K�[
        /// <summary>
        string Addt_sJobDetail { get; }
        /// <summary>
        /// �ק�
        /// <summary>
        string Modt_sJobDetail { get; }
        /// <summary>
        /// �Ұʩΰ������
        /// </summary>
        string RunOrStop { get; }
        /// <summary>
        /// ?��
        /// <summary>
        string Delt_sJobDetail { get; }
        /// <summary>
        /// ??�O�_�s�b
        /// <summary>
        string IsExitt_sJobDetail { get; }
        /// <summary>
        /// ���u�d?��^?�u��
        /// <summary>
        string GetListt_sJobDetail { get; }
        /// <summary>
        /// ���o�Ҧ�?�ƦC��
        /// <summary>
        string GetAllt_sJobDetail { get; }
    }
}
