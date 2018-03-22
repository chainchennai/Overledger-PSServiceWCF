using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PS.Data;
using log4net;



namespace PSServiceWCF
{


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class PSService : IPSService
    {
        private static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(PSService));


        
        public void SendSnapshot(PS.Data.PSSnapshotData snapshot)
        {
            try
            {
            int records = 0;
            if (snapshot != null)
            {
                _log.Debug(string.Format("Received snapshot of {0} values", snapshot.Components.Values.Count));
                foreach (PSComponent p in snapshot.Components.Values)
                {
                    records = records + p.Properties.Count;
                }

                long metricId = MetricFactory.CreateMetric(System.Environment.MachineName, typeof(PSService).ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, records);
                int snapshotId = -1;
                try
                {
                    _log.Debug("Commiting snapshot to database");
                    if (snapshot.SnapshotType == SnapshotType.FULL)
                    {
                        Console.WriteLine("Full Snapshot");
                    }
                    snapshotId = PS.Data.PS.DAO.SnapShotDAO.CommitSnapshot(snapshot);
                    _log.Debug("Committed snapshot to database");
                    MetricFactory.CommitMetric(snapshotId, metricId);
                }
                catch (Exception e)
                {
                    _log.Error("Error while commiting snapshot to database!", e);
                    MetricFactory.CommitMetricWithException(snapshotId, metricId, e.Message);
                }
                finally
                {

                }
            }

            }
            catch (Exception all)
            {
                _log.Error("Error while receiving snapshot", all);

            }
        }
    }
}
