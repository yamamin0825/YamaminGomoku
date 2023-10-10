using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//branch2 test
//bbb
//ccc
//eee

namespace YamaminGomoku
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        enum KOMA_STATUS
        { 
            NONE = 0,
            BLACK = 1,
            WHITE = 2
        }

        KOMA_STATUS[,] _enKomaStatus = new KOMA_STATUS[13, 13];

        bool _isGame = true;
        List<Ellipse> _oEllList = new List<Ellipse>();

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 13; i++)
            {
                Line oLine = new Line();
                oLine.X1 = 50 + 40 * i;
                oLine.X2 = 50 + 40 * i;
                oLine.Y1 = 50;
                oLine.Y2 = 530;
                oLine.Stroke = new SolidColorBrush(Colors.Black);

                mainCanvas.Children.Add(oLine);
            }

            for (int i = 0; i < 13; i++)
            {
                Line oLine = new Line();
                oLine.Y1 = 50 + 40 * i;
                oLine.Y2 = 50 + 40 * i;
                oLine.X1 = 50;
                oLine.X2 = 530;
                oLine.Stroke = new SolidColorBrush(Colors.Black);

                mainCanvas.Children.Add(oLine);
            }

        }

        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIElement el = sender as UIElement;
            Point p = e.GetPosition(el);

            int iXPos = (int)Math.Round( Math.Abs(p.X - 50) / 40);
            int iYPos = (int)Math.Round( Math.Abs(p.Y - 50) / 40);

            if (_isGame == false)
            {
                return;
            }

            KOMA_STATUS enKomaStatus = _enKomaStatus[iXPos, iYPos];
            if (enKomaStatus == KOMA_STATUS.NONE)
            {
                paintKoma(iXPos, iYPos, KOMA_STATUS.BLACK);
                for (int i = 1; i <= 4; i++)
                {
                    bool ret = isWinCheck(iXPos, iYPos, i);
                    if (ret == true)
                    {
                        _isGame = false;
                        MessageBox.Show("あなたの勝ちです。");
                        return;
                    }
                }
                setComputerData(iXPos, iYPos);
            }
        }

        private void paintKoma(int iXPos, int iYPos, KOMA_STATUS enKomaStatus)
        {
            Ellipse oEll = new Ellipse();
            oEll.Width = 30;
            oEll.Height = 30;

            if (enKomaStatus == KOMA_STATUS.BLACK)
            {
                oEll.Fill = new SolidColorBrush(Colors.Black);
            }
            else if (enKomaStatus == KOMA_STATUS.WHITE)
            {
                oEll.Fill = new SolidColorBrush(Colors.White);
            }

            Canvas.SetLeft(oEll, iXPos * 40 + 35);
            Canvas.SetTop(oEll, iYPos * 40 + 35);

            _oEllList.Add(oEll);

            mainCanvas.Children.Add(oEll);
            _enKomaStatus[iXPos, iYPos] = enKomaStatus;
        }

        private bool isWinCheck(int iXBlackPos,
                                int iYBlackPos,
                                int iVector)
        {
            int iXPos1 = 0;
            int iYPos1 = 0;
            int iEmptyNum1 = 0;
            int iXEmptyPos1 = 0;
            int iYEmptyPos1 = 0;
            bool isRevKoma1 = false;
            int iNum = CheckAround(iXBlackPos,
                               iYBlackPos,
                               iVector,
                               KOMA_STATUS.BLACK,
                               ref iXPos1,
                               ref iYPos1,
                               ref iEmptyNum1,
                               ref iXEmptyPos1,
                               ref iYEmptyPos1,
                               ref isRevKoma1);

            int iXPos2 = 0;
            int iYPos2 = 0;
            int iEmptyNum2 = 0;
            int iXEmptyPos2 = 0;
            int iYEmptyPos2 = 0;
            bool isRevKoma2 = false;
            iNum += CheckAround(iXBlackPos,
                                iYBlackPos,
                                iVector + 4,
                                KOMA_STATUS.BLACK,
                                ref iXPos2,
                                ref iYPos2,
                                ref iEmptyNum2,
                                ref iXEmptyPos2,
                                ref iYEmptyPos2,
                                ref isRevKoma2);


            if (iNum >= 4 && iEmptyNum1 == 0 && iEmptyNum2 == 0)
            {
                //黒の勝ち
                return true;
            }

            return false;
        }

        private bool setDefense(int iXBlackPos, 
                                int iYBlackPos, 
                                int iVector)
        {
            int iXPos1 = 0;
            int iYPos1 = 0;
            int iEmptyNum1 = 0;
            int iXEmptyPos1 = 0;
            int iYEmptyPos1 = 0;
            bool isRevKoma1 = false;
            int iNum1 = CheckAround(iXBlackPos,
                               iYBlackPos,
                               iVector,
                               KOMA_STATUS.BLACK,
                               ref iXPos1,
                               ref iYPos1,
                               ref iEmptyNum1,
                               ref iXEmptyPos1,
                               ref iYEmptyPos1,
                               ref isRevKoma1);

            int iXPos2 = 0;
            int iYPos2 = 0;
            int iEmptyNum2 = 0;
            int iXEmptyPos2 = 0;
            int iYEmptyPos2 = 0;
            bool isRevKoma2 = false;
            int iNum2 = CheckAround(iXBlackPos,
                                iYBlackPos,
                                iVector + 4,
                                KOMA_STATUS.BLACK,
                                ref iXPos2,
                                ref iYPos2,
                                ref iEmptyNum2,
                                ref iXEmptyPos2,
                                ref iYEmptyPos2,
                                ref isRevKoma2);


            if (iNum1 == 3 && iEmptyNum1 == 1)
            {
                paintKoma(iXEmptyPos1, iYEmptyPos1, KOMA_STATUS.WHITE);
                return true;
            }
            else if (iNum2 == 3 && iEmptyNum2 == 1)
            {
                paintKoma(iXEmptyPos2, iYEmptyPos2, KOMA_STATUS.WHITE);
                return true;
            }
            else if ((iNum1 + iNum2) == 3 && iEmptyNum1 == 0 && iEmptyNum2 == 0)
            {
                if (iXPos1 >= 0 && iXPos1 < 13 && iYPos1 >= 0 && iYPos1 < 13
                    && _enKomaStatus[iXPos1, iYPos1] == KOMA_STATUS.NONE)
                {
                    paintKoma(iXPos1, iYPos1, KOMA_STATUS.WHITE);
                    return true;
                }
                else if (iXPos2 >= 0 && iXPos2 < 13 && iYPos2 >= 0 && iYPos2 < 13
                    && _enKomaStatus[iXPos2, iYPos2] == KOMA_STATUS.NONE)
                {
                    paintKoma(iXPos2, iYPos2, KOMA_STATUS.WHITE);
                    return true;
                }
            }
            
            else if ((iNum1 + iNum2) == 2 && iEmptyNum1 == 0 && iEmptyNum2 == 0 && isRevKoma1 == false && isRevKoma2 == false)
            {
                if (iXPos1 >= 0 && iXPos1 < 13 && iYPos1 >= 0 && iYPos1 < 13
                    && _enKomaStatus[iXPos1, iYPos1] == KOMA_STATUS.NONE)
                {
                    paintKoma(iXPos1, iYPos1, KOMA_STATUS.WHITE);
                    return true;
                }
                else if (iXPos2 >= 0 && iXPos2 < 13 && iYPos2 >= 0 && iYPos2 < 13
                    && _enKomaStatus[iXPos2, iYPos2] == KOMA_STATUS.NONE)
                {
                    paintKoma(iXPos2, iYPos2, KOMA_STATUS.WHITE);
                    return true;
                }
            }
            else if (iEmptyNum1 == 1 && (iNum1 + iNum2) == 2 && isRevKoma1 == false && isRevKoma2 == false)
            {
                paintKoma(iXEmptyPos1, iYEmptyPos1, KOMA_STATUS.WHITE);
                return true;
            }
            else if (iEmptyNum2 == 1 && (iNum1 + iNum2) == 2 && isRevKoma1 == false && isRevKoma2 == false)
            {
                paintKoma(iXEmptyPos2, iYEmptyPos2, KOMA_STATUS.WHITE);
                return true;
            }

            return false;
        }

        private bool setAttack1(int iXWhitePos,
                                int iYWhitePos,
                                int iVector)
        {
            int iXPos1 = 0;
            int iYPos1 = 0;
            int iEmptyNum1 = 0;
            int iXEmptyPos1 = 0;
            int iYEmptyPos1 = 0;
            bool isRevKoma1 = false;
            int iNum = CheckAround(iXWhitePos,
                               iYWhitePos,
                               iVector,
                               KOMA_STATUS.WHITE,
                               ref iXPos1,
                               ref iYPos1,
                               ref iEmptyNum1,
                               ref iXEmptyPos1,
                               ref iYEmptyPos1,
                               ref isRevKoma1);

            int iXPos2 = 0;
            int iYPos2 = 0;
            int iEmptyNum2 = 0;
            int iXEmptyPos2 = 0;
            int iYEmptyPos2 = 0;
            bool isRevKoma2 = false;
            iNum += CheckAround(iXWhitePos,
                                iYWhitePos,
                                iVector + 4,
                                KOMA_STATUS.WHITE,
                                ref iXPos2,
                                ref iYPos2,
                                ref iEmptyNum2,
                                ref iXEmptyPos2,
                                ref iYEmptyPos2,
                                ref isRevKoma2);


            if (iNum == 4 && iEmptyNum1 == 0 && iEmptyNum2 == 0)
            {
                //白の勝ち
                paintKoma(iXWhitePos, iYWhitePos, KOMA_STATUS.WHITE);
                _isGame = false;
                MessageBox.Show("あなたの負けです。");
                return true;
            }

            return false;
        }

        private bool setAttack2(int iXWhitePos,
                                int iYWhitePos,
                                int iVector)
        {
            int iXPos1 = 0;
            int iYPos1 = 0;
            int iEmptyNum1 = 0;
            int iXEmptyPos1 = 0;
            int iYEmptyPos1 = 0;
            bool isRevKoma1 = false;
            int iNum = CheckAround(iXWhitePos,
                               iYWhitePos,
                               iVector,
                               KOMA_STATUS.WHITE,
                               ref iXPos1,
                               ref iYPos1,
                               ref iEmptyNum1,
                               ref iXEmptyPos1,
                               ref iYEmptyPos1,
                               ref isRevKoma1);

            int iXPos2 = 0;
            int iYPos2 = 0;
            int iEmptyNum2 = 0;
            int iXEmptyPos2 = 0;
            int iYEmptyPos2 = 0;
            bool isRevKoma2 = false;
            iNum += CheckAround(iXWhitePos,
                                iYWhitePos,
                                iVector + 4,
                                KOMA_STATUS.WHITE,
                                ref iXPos2,
                                ref iYPos2,
                                ref iEmptyNum2,
                                ref iXEmptyPos2,
                                ref iYEmptyPos2,
                                ref isRevKoma2);


            if (iNum == 3 && iEmptyNum1 == 0 && iEmptyNum2 == 0 && isRevKoma1 == false && isRevKoma2 == false)
            {
                paintKoma(iXWhitePos, iYWhitePos, KOMA_STATUS.WHITE);
                return true;
            }
            else if (iEmptyNum1 == 1 && iNum == 3 && isRevKoma1 == false && isRevKoma2 == false)
            {
                paintKoma(iXEmptyPos1, iYEmptyPos1, KOMA_STATUS.WHITE);
                return true;
            }
            else if (iEmptyNum2 == 1 && iNum == 3 && isRevKoma1 == false && isRevKoma2 == false)
            {
                paintKoma(iXEmptyPos2, iYEmptyPos2, KOMA_STATUS.WHITE);
                return true;
            }

            return false;
        }

        private bool setAttack3(int iXWhitePos,
                                int iYWhitePos,
                                int iVector)
        {
            int iXPos1 = 0;
            int iYPos1 = 0;
            int iEmptyNum1 = 0;
            int iXEmptyPos1 = 0;
            int iYEmptyPos1 = 0;
            bool isRevKoma1 = false;
            int iNum = CheckAround(iXWhitePos,
                               iYWhitePos,
                               iVector,
                               KOMA_STATUS.WHITE,
                               ref iXPos1,
                               ref iYPos1,
                               ref iEmptyNum1,
                               ref iXEmptyPos1,
                               ref iYEmptyPos1,
                               ref isRevKoma1);

            int iXPos2 = 0;
            int iYPos2 = 0;
            int iEmptyNum2 = 0;
            int iXEmptyPos2 = 0;
            int iYEmptyPos2 = 0;
            bool isRevKoma2 = false;
            iNum += CheckAround(iXWhitePos,
                                iYWhitePos,
                                iVector + 4,
                                KOMA_STATUS.WHITE,
                                ref iXPos2,
                                ref iYPos2,
                                ref iEmptyNum2,
                                ref iXEmptyPos2,
                                ref iYEmptyPos2,
                                ref isRevKoma2);


            if (iNum == 2 && iEmptyNum1 == 0 && iEmptyNum2 == 0 && isRevKoma1 == false && isRevKoma2 == false)
            {
                paintKoma(iXWhitePos, iYWhitePos, KOMA_STATUS.WHITE);
                return true;
            }
            else if (iEmptyNum1 == 1 && iNum == 2 && isRevKoma1 == false && isRevKoma2 == false)
            {
                paintKoma(iXEmptyPos1, iYEmptyPos1, KOMA_STATUS.WHITE);
                return true;
            }
            else if (iEmptyNum2 == 1 && iNum == 2 && isRevKoma1 == false && isRevKoma2 == false)
            {
                paintKoma(iXEmptyPos2, iYEmptyPos2, KOMA_STATUS.WHITE);
                return true;
            }
            
            return false;
        }

        private int setAttack4(int iXWhitePos,
                                int iYWhitePos,
                                int iVector)
        {
            int iXPos1 = 0;
            int iYPos1 = 0;
            int iEmptyNum1 = 0;
            int iXEmptyPos1 = 0;
            int iYEmptyPos1 = 0;
            bool isRevKoma1 = false;
            int iNum = CheckAround(iXWhitePos,
                               iYWhitePos,
                               iVector,
                               KOMA_STATUS.BLACK,
                               ref iXPos1,
                               ref iYPos1,
                               ref iEmptyNum1,
                               ref iXEmptyPos1,
                               ref iYEmptyPos1,
                               ref isRevKoma1);

            int iXPos2 = 0;
            int iYPos2 = 0;
            int iEmptyNum2 = 0;
            int iXEmptyPos2 = 0;
            int iYEmptyPos2 = 0;
            bool isRevKoma2 = false;
            iNum += CheckAround(iXWhitePos,
                                iYWhitePos,
                                iVector + 4,
                                KOMA_STATUS.BLACK,
                                ref iXPos2,
                                ref iYPos2,
                                ref iEmptyNum2,
                                ref iXEmptyPos2,
                                ref iYEmptyPos2,
                                ref isRevKoma2);

            if (iEmptyNum1 == 0 && iEmptyNum2 == 0)
                return iNum;
            else
                return 0;
            
        }

        private void setComputerData(int iXBlackPos, int iYBlackPos)
        {
            // -------------------------
            // 白の勝ち
            // -------------------------
            for (int x = 0; x < 13; x++)
            {
                for(int y = 0; y < 13; y++)
                {
                    if (_enKomaStatus[x, y] != KOMA_STATUS.NONE)
                    {
                        continue;
                    }
                    for (int i = 1; i <= 4; i++)
                    {
                        bool ret = setAttack1(x, y, i);
                        if (ret == true)
                            return;
                    }
                }
            }
            
            // -------------------------
            // 黒を阻止する
            // -------------------------
            for (int i = 1; i <= 4; i++)
            {
                bool ret = setDefense(iXBlackPos, iYBlackPos, i);
                if (ret == true)
                    return;
            }

            for (int x = 0; x < 13; x++)
            {
                for (int y = 0; y < 13; y++)
                {
                    if (_enKomaStatus[x, y] != KOMA_STATUS.NONE)
                    {
                        continue;
                    }
                    for (int i = 1; i <= 4; i++)
                    {
                        bool ret = setAttack2(x, y, i);
                        if (ret == true)
                            return;
                    }
                }
            }

            for (int x = 0; x < 13; x++)
            {
                for (int y = 0; y < 13; y++)
                {
                    if (_enKomaStatus[x, y] != KOMA_STATUS.NONE)
                    {
                        continue;
                    }
                    for (int i = 1; i <= 4; i++)
                    {
                        bool ret = setAttack3(x, y, i);
                        if (ret == true)
                            return;
                    }
                }
            }

            int iMaxNum = 0;
            int iXSetPos = 0;
            int iYSetPos = 0;
            for (int x = 0; x < 13; x++)
            {
                for (int y = 0; y < 13; y++)
                {
                    if (_enKomaStatus[x, y] != KOMA_STATUS.NONE)
                    {
                        continue;
                    }
                    int iNum = 0;
                    for (int i = 1; i <= 4; i++)
                    {
                        iNum += setAttack4(x, y, i);
                    }
                    if (iMaxNum < iNum)
                    {
                        iMaxNum = iNum;
                        iXSetPos = x;
                        iYSetPos = y;
                    }
                }
            }
            paintKoma(iXSetPos, iYSetPos, KOMA_STATUS.WHITE);
        }
        
        int CheckAround(int iXPos, 
                        int iYPos, 
                        int iVector, 
                        KOMA_STATUS enKomaStatus,
                        ref int iXCulPos,
                        ref int iYCulPos,
                        ref int iEmptyNum,
                        ref int iXEmptyPos,
                        ref int iYEmptyPos,
                        ref bool isRevKoma)
        {
            int iNum = 0;
            int iEmptyCulNum = 0;

            KOMA_STATUS enSearchKomaStatus = KOMA_STATUS.NONE;
            iXCulPos = iXPos;
            iYCulPos = iYPos;

            bool isBreak = false;
            while (true)
            {
                switch (iVector)
                {
                    case 1:
                        --iYCulPos;
                        if (iYCulPos < 0) 
                            isBreak = true;
                        else
                            enSearchKomaStatus = _enKomaStatus[iXCulPos, iYCulPos];
                        break;

                    case 2:
                        ++iXCulPos;
                        --iYCulPos;
                        if (iXCulPos > 12 || iYCulPos < 0)
                            isBreak = true;
                        else
                            enSearchKomaStatus = _enKomaStatus[iXCulPos, iYCulPos];
                        break;

                    case 3:
                        ++iXCulPos;
                        if (iXCulPos > 12)
                            isBreak = true;
                        else
                            enSearchKomaStatus = _enKomaStatus[iXCulPos, iYCulPos];
                        break;

                    case 4:
                        ++iXCulPos;
                        ++iYCulPos;
                        if (iXCulPos > 12 || iYCulPos > 12)
                            isBreak = true;
                        else
                            enSearchKomaStatus = _enKomaStatus[iXCulPos, iYCulPos];
                        break;

                    case 5:
                        ++iYCulPos;
                        if (iYCulPos > 12)
                            isBreak = true;
                        else
                            enSearchKomaStatus = _enKomaStatus[iXCulPos, iYCulPos];
                        break;

                    case 6:
                        --iXCulPos;
                        ++iYCulPos;
                        if (iXCulPos < 0 || iYCulPos > 12)
                            isBreak = true;
                        else
                            enSearchKomaStatus = _enKomaStatus[iXCulPos, iYCulPos];
                        break;

                    case 7:
                        --iXCulPos;
                        if (iXCulPos < 0)
                            isBreak = true;
                        else
                            enSearchKomaStatus = _enKomaStatus[iXCulPos, iYCulPos];
                        break;

                    case 8:
                        --iXCulPos;
                        --iYCulPos;
                        if (iXCulPos < 0 || iYCulPos < 0)
                            isBreak = true;
                        else
                            enSearchKomaStatus = _enKomaStatus[iXCulPos, iYCulPos];
                        break;
                }

                if (isBreak == true)
                    break; 

                if (enSearchKomaStatus == KOMA_STATUS.NONE)
                {
                    if (iEmptyNum == 0)
                    {
                        iXEmptyPos = iXCulPos;
                        iYEmptyPos = iYCulPos;
                    }
                    iEmptyNum++;
                    iEmptyCulNum++;
                    if (iEmptyCulNum > 1)
                    {
                        break;
                    }
                }

                else if (enKomaStatus == enSearchKomaStatus)
                {
                    iNum++;
                    iEmptyCulNum = 0;
                }
                else if (enSearchKomaStatus != KOMA_STATUS.NONE)
                {
                    isRevKoma = true;
                    iEmptyCulNum = 0;
                    break;
                }
            }

            if (iEmptyCulNum > 0)
            {
                iXCulPos = iXEmptyPos;
                iYCulPos = iYEmptyPos;
                iEmptyNum -= iEmptyCulNum;
            }
            return iNum;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach(var oEll in _oEllList)
            {
                mainCanvas.Children.Remove(oEll);
            }
            _oEllList.RemoveRange(0, _oEllList.Count);
            _isGame = true;
            _enKomaStatus = new KOMA_STATUS[13, 13];
        }
    }
}

