using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace subscription.Models
{
    public class UserSubscriptions
    {
       public string _id { get; set; }
       public Subscribee[] subscribees { get; set; }
       public string _rev { get; set; }
        public int AddSubscribee(Subscribee s)
        {
            this.subscribees.ToList().Add(s);
            return 1;

           
        }
        public int RemoveSubscribee(Subscribee s)
        {
            this.subscribees.ToList().Remove(s);
            return 1;
        }
    }
}
 
