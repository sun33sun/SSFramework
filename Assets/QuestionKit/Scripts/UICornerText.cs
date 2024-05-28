using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace LTYFrameWork.UI
{
    //这个代码是我抄的（嘻嘻
    [ExecuteInEditMode]
    public class UICornerText : Text
    {
        /// <summary>
        /// 正则表达式
        /// </summary>
        private static readonly string m_RegexTag = @"\[([1-4]{1})(\|([0-9]{1,3}))?(\|([0-9A-Za-z]+))?=(.+?)\]";
        private static readonly Regex m_Regex = new Regex(m_RegexTag, RegexOptions.Singleline);
        /// <summary>
        /// 文本转换
        /// </summary>
        private static readonly StringBuilder m_TextBuilder = new StringBuilder();
        /// <summary>
        /// 顶点列表
        /// </summary>
        private readonly UIVertex[] m_TempVerts = new UIVertex[4];
        /// <summary>
        /// 输出文本
        /// </summary>
        private string m_OutputText;

        /// <summary>
        /// 字符宽度
        /// </summary>
        public float CharWidth { get; private set; }
        /// <summary>
        /// 字符高度
        /// </summary>
        public float CharHeight { get; private set; }

        /// <summary>
        /// 角标数据
        /// </summary>
        private List<CornerInfo> m_cornerBoxInfos = new List<CornerInfo>();

        /// <summary>
        /// 角标高度偏移值
        /// </summary>
        [Range(-1, 1)]
        public float heightOffset = 0.08f;

        public override void SetVerticesDirty()
        {
//#if UNITY_EDITOR
//            m_OutputText = GetOutputText();
//#endif
//            base.SetVerticesDirty();

            m_OutputText = GetOutputText();
            base.SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (font == null)
                return;

            Vector2 extents = rectTransform.rect.size;
            var settings = GetGenerationSettings(extents);
            cachedTextGenerator.Populate(m_OutputText, settings);

            IList<UIVertex> verts = cachedTextGenerator.verts;
            float unitsPerPixel = 1 / pixelsPerUnit;
            int vertCount = verts.Count - 4;
            if (vertCount <= 0)
            {
                toFill.Clear();
                return;
            }

            Vector2 roundingOffset = new Vector2(verts[0].position.x, verts[0].position.y) * unitsPerPixel;
            roundingOffset = PixelAdjustPoint(roundingOffset) - roundingOffset;
            toFill.Clear();

            List<Vector3> _vertsPosList = new List<Vector3>();

            if (roundingOffset != Vector2.zero)
            {
                for (int i = 0; i < vertCount; ++i)
                {
                    int tempVertsIndex = i & 3;
                    m_TempVerts[tempVertsIndex] = verts[i];
                    m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                    m_TempVerts[tempVertsIndex].position.x += roundingOffset.x;
                    m_TempVerts[tempVertsIndex].position.y += roundingOffset.y;
                    if (tempVertsIndex == 3)
                        toFill.AddUIVertexQuad(m_TempVerts);
                    _vertsPosList.Add(m_TempVerts[tempVertsIndex].position);
                }
            }
            else
            {
                for (int i = 0; i < vertCount; ++i)
                {
                    int tempVertsIndex = i & 3;
                    m_TempVerts[tempVertsIndex] = verts[i];
                    m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                    if (tempVertsIndex == 3)
                        toFill.AddUIVertexQuad(m_TempVerts);
                    _vertsPosList.Add(m_TempVerts[tempVertsIndex].position);
                }
            }

            DrawCornerScript(toFill, m_cornerBoxInfos);
        }

        private string GetOutputText()
        {

            m_TextBuilder.Clear();
            m_cornerBoxInfos.Clear();
            int unMatchIndex = 0;

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            string inputText = text;

            Vector2 extents = rectTransform.rect.size;
            TextGenerationSettings settings = GetGenerationSettings(extents);

            CharWidth = cachedTextGeneratorForLayout.GetPreferredWidth(inputText, settings) / this.pixelsPerUnit;
            if (cachedTextGeneratorForLayout.GetCharactersArray().Length > 0)
                CharWidth = cachedTextGeneratorForLayout.GetCharactersArray()[0].charWidth;

            CharHeight = cachedTextGeneratorForLayout.GetPreferredHeight(inputText, settings) / this.pixelsPerUnit;
            if (cachedTextGeneratorForLayout.GetLinesArray().Length > 0)
                CharHeight = cachedTextGeneratorForLayout.GetLinesArray()[0].height;

            foreach (Match match in m_Regex.Matches(inputText))
            {
                string _type = match.Groups[1].Value;

                bool useSquarebrackets = _type == "3" || _type == "4" ? true : false;

                int _fontSize = fontSize;
                if (match.Groups[3].Success)
                {
                    string _size = match.Groups[3].Value;
                    _fontSize = int.Parse(_size);
                }

                string _color = string.Empty;
                if (match.Groups[5].Success)
                    _color = match.Groups[5].Value;

                Color _contentColor = GetColor(_color, color);
                string color1 = GetColor(_contentColor);

                string _content = match.Groups[6].Value;

                m_TextBuilder.Append(inputText.Substring(unMatchIndex, match.Index - unMatchIndex));
                int _startIndex = m_TextBuilder.Length * 4;
                m_TextBuilder.Append("<color=#");
                m_TextBuilder.Append(color1);
                m_TextBuilder.Append(">");
                m_TextBuilder.Append("<size=");
                m_TextBuilder.Append(Mathf.FloorToInt(_fontSize / 2f));
                m_TextBuilder.Append(">");
                if (useSquarebrackets)
                    m_TextBuilder.Append("[");
                m_TextBuilder.Append(_content);
                if (useSquarebrackets)
                    m_TextBuilder.Append("]");
                m_TextBuilder.Append("</size>");
                m_TextBuilder.Append("</color>");
                int _endIndex = m_TextBuilder.Length * 4;

                CornerInfo cornerInfo = new CornerInfo()
                {
                    id = _type,
                    startIndex = _startIndex,
                    endIndex = _endIndex,
                };
                m_cornerBoxInfos.Add(cornerInfo);

                unMatchIndex = match.Index + match.Length;
            }

            m_TextBuilder.Append(inputText.Substring(unMatchIndex, inputText.Length - unMatchIndex));
            return m_TextBuilder.ToString();
        }

        /// <summary>
        /// 画角标
        /// </summary>
        /// <param name="_vertsPosList"></param>
        private void DrawCornerScript<T>(VertexHelper toFill, List<T> boxInfo)
            where T : CornerInfo
        {
            if (boxInfo.Count <= 0)
                return;

            List<UIVertex> vertexs = new List<UIVertex>();
            toFill.GetUIVertexStream(vertexs);
            int vertexCount = vertexs.Count;
            foreach (var item in boxInfo)
            {
                int _startIndex = item.startIndex / 4;
                int _endIndex = item.endIndex / 4;
                switch (item.id)
                {
                    case "1": //上标
                    case "3": 
                        for (int i = 0; i < _endIndex - _startIndex; i++)
                        {
                            int index = _startIndex + i;
                            for (int j = 0; j < 6; j++)
                            {
                                if (vertexCount < ((index + 1) * 6))
                                    break;
                                UIVertex uIVertex = vertexs[index * 6 + j];
                                Vector3 pos = uIVertex.position + new Vector3(0, CharHeight * (0.25f + heightOffset), 0);
                                uIVertex.position = pos;
                                vertexs[index * 6 + j] = uIVertex;
                            }
                        }
                        break;
                    case "2": //下标
                    case "4":
                    default:
                        break;
                }
            }

            toFill.Clear();
            toFill.AddUIVertexTriangleStream(vertexs);
        }

        #region 辅助功能

        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="color">16进制字符/颜色</param>
        /// <param name="originalColor"></param>
        /// <returns></returns>
        private Color GetColor(string color, Color originalColor)
        {
            Color _newColor = originalColor;
            if (!string.IsNullOrEmpty(color))
            {
                bool IsDefined = ColorUtility.TryParseHtmlString(color, out _newColor);
                if (!IsDefined)
                {
                    if (!color.Contains("#"))
                        color = string.Format("#{0}", color);
                    IsDefined = ColorUtility.TryParseHtmlString(color, out _newColor);
                    if (!IsDefined)
                        _newColor = base.color;
                }
            }
            return _newColor;
        }
        /// <summary>
        /// 获取16进制颜色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private string GetColor(Color color)
        {
            return ColorUtility.ToHtmlStringRGBA(color);
        }

        private class CornerInfo
        {
            public string id;
            public int startIndex;
            public int endIndex;
        }

        #endregion
        
    }
}