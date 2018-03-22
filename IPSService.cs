using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PS.Data;





namespace PSServiceWCF
{
    [ServiceContract]
    
    public interface IPSService
    {
        [OperationContract(IsOneWay = true)]
        void SendSnapshot(PSSnapshotData snapshot);
    }

   
}
