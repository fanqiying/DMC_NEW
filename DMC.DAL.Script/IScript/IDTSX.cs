namespace DMC.DAL.Script
{
    public interface IDTSX
    {
        /// <summary>
        /// �K�[
        /// <summary>
        string Addt_sDTSX { get; }
        /// <summary>
        /// �ק�
        /// <summary>
        string Modt_sDTSX { get; }
        /// <summary>
        /// ?��
        /// <summary>
        string Delt_sDTSX { get; }
        string DelTaskIDList { get; }
        /// <summary>
        /// ??�O�_�s�b
        /// <summary>
        string IsExitt_sDTSX { get; }
        /// <summary>
        /// ���u�d?��^?�u��
        /// <summary>
        string GetListt_sDTSX { get; }
        /// <summary>
        /// ���o�Ҧ�?�ƦC��
        /// <summary>
        string GetAllt_sDTSX { get; }
        /// <summary>
        /// ���DTSX�������B�J
        /// </summary>
        string GetDtsxSteps { get; }
    }
}
