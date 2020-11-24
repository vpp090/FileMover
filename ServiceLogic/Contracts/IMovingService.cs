using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLogic.Contracts
{
    public interface IMovingService
    {
        void Move(string sourceLocation, string destinationLocation);
    }
}
