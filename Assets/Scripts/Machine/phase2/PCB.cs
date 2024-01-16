using Unity.VisualScripting;

public class PCB
    {
        int PID;
        int TTL, TLL;
        int TTC, LLC;
        public PCB(int pid, int ttl, int tll, int ttc, int llc)
        {
            this.PID = pid;
            this.TTL = ttl;
            this.TLL = tll;
            this.TTC = ttc;
            this.LLC = llc;
        }

        #region GET Functions

        public int get_pid()
        {
            return PID;
        }

        public int get_TTL()
        {
            return TTL;
        }

        public int get_TLL()
        {
            return TLL;
        }

        public int get_TTC()
        {
            return TTC;
        }

        public int get_LLC()
        {
            return LLC;
        }

        #endregion
    
        #region SET functions

        public void set_LLC(int llc)
        {
            this.LLC = llc;
        }

        public void set_TTC(int ttc)
        {
            this.TTC = ttc;
        }

        #endregion
    }