using System.Globalization;
using System.Text;
using umbraco.BusinessLogic;
using umbraco.cms.presentation.Trees;
using umbraco.businesslogic;

namespace Khaaaantest.Tree
{
    [Tree("contests", "contests", "Contests")]
    public class LoadContests:BaseTree
    {
        public LoadContests(string application):base(application){}

        public override void RenderJS(ref StringBuilder javascript)
        {
            javascript.Append(
                                @"
                                 function openContest(id)
                                 {
                                     parent.right.document.location.href = 'plugins/Khaaaantest/Contest.aspx?id='+id;
                                 }
                                 ");
        }

        public override void Render(ref XmlTree tree)
        {
            var reader = Application.SqlHelper.ExecuteReader("select * from khaaaantest_Contests");
            while (reader.Read())
            {
                var node = XmlTreeNode.Create(this);
                node.NodeID = reader.GetInt("Id").ToString(CultureInfo.InvariantCulture);
                node.Text = reader.GetString("Name");
                node.Icon = "contest.png";
                node.Action = "javascript:openContest(" + reader.GetInt("Id").ToString(CultureInfo.InvariantCulture) + ")";
                tree.Add(node);
            }
        }

        protected override void CreateRootNode(ref XmlTreeNode rootNode)
        {
            rootNode.Icon = FolderIcon;
            rootNode.OpenIcon = FolderIconOpen;
            rootNode.NodeType = TreeAlias;
            rootNode.NodeID = "init";
        }
    }
}