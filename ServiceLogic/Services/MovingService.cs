using ServiceLogic.Contracts;
using System.IO;

namespace ServiceLogic.Services
{
    public class MovingService : IMovingService
    {
        public void Move(string sourceLocation, string destinationLocation)
        {
            if (!Directory.Exists(destinationLocation))
                Directory.CreateDirectory(destinationLocation);

            var files = Directory.GetFiles(sourceLocation);

            foreach(var file in files)
            {
                var fileName = Path.GetFileName(file);

                var destination = Path.Combine(destinationLocation, fileName);
                File.Copy(file, destination, true);
            }
        }
    }
}
