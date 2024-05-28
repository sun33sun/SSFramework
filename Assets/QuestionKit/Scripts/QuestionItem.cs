using System;
using System.Collections;
using System.Collections.Generic;
using LTYFrameWork.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ProjectBase.Exam
{
    public class QuestionItem : MonoBehaviour
    {
        public Text txtHead;

        public RectTransform gridOption;

        public static GameObject optionPrefab;
        
        public Text txtAnalysis;

        [Header("选项变化的颜色")] private Color _colorNormal;
        private Color _colorSelected;
        private Color _colorError;
        private Color _colorCorrect;
        private Color _colorAnalysisError;
        private Color _colorAnalysisCorrect;

        [Header("_所有选项")] public List<Toggle> _allSelection = new List<Toggle>();
        List<UICornerText> _alltxtSelection = new List<UICornerText>();

        public void OnEnable()
        {
            txtAnalysis.text = "";
        }

        public void SetColor(Color colorNormal, Color colorSelected, Color colorError, Color colorCorrect,
            Color colorAnalysisError, Color colorAnalysisCorrect)
        {
            _colorNormal = colorNormal;
            _colorSelected = colorSelected;
            _colorError = colorError;
            _colorCorrect = colorCorrect;
            _colorAnalysisError = colorAnalysisError;
            _colorAnalysisCorrect = colorAnalysisCorrect;
        }

        public void SetQuestion(string strHead, List<string> optionList, QuestionData.ChoiceType choiceTypeType,GameObject prefab)
        {
            optionPrefab = prefab;
            txtHead.text = strHead + "E";
            txtHead.color = _colorNormal;
            GameObject go = null;
            ToggleGroup group = null;
            if (choiceTypeType == QuestionData.ChoiceType.Single)
            {
                group = gridOption.gameObject.AddComponent<ToggleGroup>();
                group.allowSwitchOff = true;
            }

            for (int i = 0; i < optionList.Count; i++)
            {
                go = Instantiate(optionPrefab, gridOption);
                //选项文字
                _alltxtSelection.Add(go.transform.Find("txtOption").GetComponent<UICornerText>());
                _alltxtSelection[i].text = optionList[i];
                _alltxtSelection[i].color = _colorNormal;
                //选项
                _allSelection.Add(go.GetComponent<Toggle>());
                _allSelection[i].group = group;
                _allSelection[i].isOn = false;
                //添加订阅方法
                AddToggleCallBack(i);
            }
        }

        void AddToggleCallBack(int togIndex)
        {
            _allSelection[togIndex].onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    _allSelection[togIndex].graphic.color = _colorSelected;
                    _allSelection[togIndex].targetGraphic.color = _colorSelected;
                    _alltxtSelection[togIndex].color = _colorSelected;
                }
                else
                {
                    _allSelection[togIndex].graphic.color = Color.white;
                    _allSelection[togIndex].targetGraphic.color = Color.white;
                    _alltxtSelection[togIndex].color = _colorNormal;
                }
            });
        }
        
        public float SingleJudge(QuestionData.ChoiceIndex rightOption, string analysis, float score)
        {
            int rightIndex = rightOption.GetHashCode() - 1;
            for (int i = 0; i < _allSelection.Count; i++)
            {
                if (_allSelection[i].isOn)
                {
                    _allSelection[i].graphic.color = Color.red;
                    _allSelection[i].targetGraphic.color = Color.red;
                    _alltxtSelection[i].color = Color.red;
                }

                _allSelection[i].interactable = false;
            }

            _allSelection[rightIndex].graphic.color = _colorCorrect;
            _allSelection[rightIndex].targetGraphic.color = _colorCorrect;
            _alltxtSelection[rightIndex].color = _colorCorrect;
            
            if (_allSelection[rightIndex].isOn)
            {
                txtAnalysis.text = "回答正确！" + analysis.ToString() + "E";
                txtAnalysis.color = _colorAnalysisCorrect;
                return score;
            }
            else
            {
                txtAnalysis.text = "回答错误！" + "正确答案为：" + rightOption.ToString() + "、" + analysis.ToString() + "E";
                txtAnalysis.color = _colorAnalysisError;
                return 0;
            }
        }

        public float MultipleJudge(List<QuestionData.ChoiceIndex> _rightList, string _解析内容, float _该题分值)
        {
            int I = 0;
            string X = "";
            for (int i = 0; i < _allSelection.Count; i++)
            {
                //选中的选项全部变为红色
                if (_allSelection[i].isOn)
                {
                    _allSelection[i].graphic.color = _colorError;
                    _allSelection[i].targetGraphic.color = _colorError;
                    _alltxtSelection[i].color = _colorError;
                }

                //关闭交互
                _allSelection[i].interactable = false;
            }

            for (int i = 0; i < _rightList.Count; i++)
            {
                //将正确选项的颜色变为绿色
                _allSelection[i].graphic.color = _colorCorrect;
                _allSelection[i].targetGraphic.color = _colorCorrect;
                _alltxtSelection[i].color = _colorCorrect;
                //检查正确选项是否选中
                if (_allSelection[_rightList[i].GetHashCode() - 1].isOn)
                {
                    I++;
                }

                //计算解析
                X = X + _rightList[i].ToString() + "、";
            }

            //显示解析
            if (I == _rightList.Count)
            {
                txtAnalysis.text = "回答正确！" + _解析内容.ToString() + "E";
                txtAnalysis.color = _colorCorrect;
                return _该题分值;
            }
            else
            {
                txtAnalysis.text = "回答错误！" + "正确答案为：" + X + _解析内容.ToString() + "E";
                txtAnalysis.color = _colorError;
                return 0;
            }
        }

        public void Reset()
        {
            txtAnalysis.text = "";
            txtAnalysis.color = _colorNormal;

            for (int i = 0; i < _allSelection.Count; i++)
            {
                _allSelection[i].interactable = true;
                _allSelection[i].isOn = false;
                _allSelection[i].graphic.color = Color.white;
                _allSelection[i].targetGraphic.color = Color.white;
                _alltxtSelection[i].color = _colorNormal;
            }
        }
    }
}