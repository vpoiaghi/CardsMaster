﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Windows;
using System.Collections.ObjectModel;

using System.Windows.Media;

namespace CardMasterStat
{
    class CardComputer
    {
        private List<Card> m_listCards;

        public CardComputer(List<Card> list)
        {
            this.m_listCards = list;
        }

        public IEnumerable GetRepartitionByCost()
        {
            List<Point> toReturn = new List<Point>();
            for (int i = 0; i < 10; i++)
            {
                toReturn.Add(new Point(i, this.m_listCards.Where(c => c.Cost == i).Count()));
            }
            return toReturn;
        }

        public IEnumerable GetRepartitionByAttack()
        {
            List<Point> toReturn = new List<Point>();
            for (int i = 0; i < 10; i++)
            {
                toReturn.Add(new Point(i, this.m_listCards.Where(c => c.Attack == i).Count()));
            }
            return toReturn;
        }

        public IEnumerable GetRepartitionByDefense()
        {
            List<Point> toReturn = new List<Point>();
            for (int i = 0; i < 10; i++)
            {
                toReturn.Add(new Point(i, this.m_listCards.Where(c => c.Defense == i).Count()));
            }
            return toReturn;
            
        }

        internal IEnumerable GetLowerLineRatio()
        {
            List<Point> toReturn = new List<Point>
            {
                new Point(1, 0),
                new Point(9, 10)
            };
            return toReturn;

        }

        internal IEnumerable GetHigherLineRatio()
        {
            List<Point> toReturn = new List<Point>
            {
                new Point(0, 5),
                new Point(9, 25)
            };
            return toReturn;
        }

        public IEnumerable GetRepartitionByRatio()
        {
            return m_listCards;
        }


        public IEnumerable GetRepartitionByNature()
        {
            SortedDictionary<String, int> toReturn = new SortedDictionary<String, int>();
            foreach (Card.CardKind kind in Enum.GetValues(typeof(Card.CardKind)))
            {
                if (kind.Equals(Card.CardKind.Ninja))
                {
                    foreach (Card.NatureCard nature in Enum.GetValues(typeof(Card.NatureCard)))
                    {
                        toReturn.Add(kind.ToString() + " " + nature.ToString(), this.m_listCards.Where(c => c.Kind == kind && c.Nature == nature).Count());
                    }
                        
                }
                else
                {
                    toReturn.Add(kind.ToString(), this.m_listCards.Where(c => c.Kind == kind).Count());
                }
               
            }
            return toReturn;
        }

        public int GetTotalCount()
        {
            return this.m_listCards.Count;
        }


    }
}
