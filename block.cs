using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TabControlExperiments
{
    public class Block : INotifyPropertyChanged
    {
        private const string signKey = "0000";


        
        #region private strings

        private string id;
        public string ID { get => id; set { id = value; NotifyPropertyChanged(); } }

        private string data;
        public string Data { get => data; set { data = value; NotifyPropertyChanged(); } }

        private string hash;
        public string Hash { get => hash; set { hash = value; NotifyPropertyChanged(); } }

        private string nonce;
        public string Nonce { get => nonce; set { nonce = value; NotifyPropertyChanged(); } }

        private string previousHash;
        public string PreviousHash { get => previousHash; set { previousHash = value; NotifyPropertyChanged(); } }
        #endregion

        /// <summary>
        /// sets nonce and previoushash to a set number
        /// </summary>
        public Block(string ID)
        {
            this.ID = ID;
            Nonce = "0";
            Data = string.Empty;
            PreviousHash = "0000";
            
            PropertyChanged += InternalHandler;
            

        }

        /// <summary>
        /// The code that uses IsMining to mine the hash
        /// Increases the nonce by one
        /// </summary>
        public void Mine()
        {
            if (Block.IsMining) return;
            Block.IsMining = true;
            nonce = "0";
            while (!IsSigned)
                Nonce = (int.Parse(Nonce) + 1).ToString();
            

            IsMining = false;
            NotifyPropertyChanged("Hash");
        }


        private void InternalHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Hash" || e.PropertyName == "IsSigned") return;
            
            this.ReHash();
        }


        private void ReHash()
        {
            Hash = HashConverter.HashGen(ID, Nonce, Data, PreviousHash);
            PropertyChanged(this, new PropertyChangedEventArgs("IsSigned"));
        }


        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
        public bool IsSigned => string.Equals(Hash.Substring(0, signKey.Length), signKey);

        public event PropertyChangedEventHandler PropertyChanged;

        public static bool IsMining { get; private set; } = false;

        


    }

    /// <summary>
    /// Code that is returned if the block is already mining
    /// </summary>
    public class BlockChain
    {
        public ObservableCollection<Block> Blocks { get; set; } = new ObservableCollection<Block>();

        public void Add(Block B)
        {
            B.PreviousHash = B.ID.Equals("0") ? "000000000000" : Blocks[Blocks.Count - 1].Hash;
            Blocks.Add(B);
            B.PropertyChanged += InternalHandler;
        }
        private void InternalHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Hash") return;
            if (sender is Block B)
            {
                if (Block.IsMining) return;

                int index = int.Parse(B.ID);
                if (index + 1 == Blocks.Count) return;
                Blocks[index + 1].PreviousHash = Blocks[index].Hash;
            }
        }
    }
}  
