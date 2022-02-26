using System.Runtime.Serialization;
using System.ServiceModel;

namespace Examples.WCF
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IService1" を変更できます。
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: ここにサービス操作を追加します。
    }

    // サービス操作に複合型を追加するには、以下のサンプルに示すようにデータ コントラクトを使用します。
    // プロジェクトに XSD ファイルを追加できます。プロジェクトのビルド後、そこで定義されたデータ型を、名前空間 "Examples.WCF.ContractType" で直接使用できます。
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
