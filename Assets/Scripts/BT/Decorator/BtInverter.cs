using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG.BT
{
    public class BtInverter : BtDecorator
    {
        protected override EStatus OnUpdate()
        {
            child.Tick();
            if (child.IsFailure)
                return EStatus.Success;
            if (child.IsSuccess)
                return EStatus.Failure;
            return EStatus.Running;
        }
    }
}