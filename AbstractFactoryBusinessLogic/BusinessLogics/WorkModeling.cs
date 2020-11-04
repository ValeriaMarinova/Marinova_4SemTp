﻿using System;
using System.Collections.Generic;
using AbstractFactoryBusinessLogic.BindingModels;
using AbstractFactoryBusinessLogic.Interfaces;
using AbstractFactoryBusinessLogic.ViewModels;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AbstractFactoryBusinessLogic.BusinessLogics
{
    public class WorkModeling
    {
        private readonly IImplementerLogic implementerLogic;
        private readonly IOrderLogic orderLogic;
        private readonly MainLogic mainLogic;

        private readonly Random rnd;

        public WorkModeling(IImplementerLogic implementerLogic, IOrderLogic orderLogic, MainLogic mainLogic)
        {
            this.implementerLogic = implementerLogic;
            this.orderLogic = orderLogic;
            this.mainLogic = mainLogic;
            rnd = new Random(1000);
        }


        public void DoWork()
        {
            var implementers = implementerLogic.Read(null);
            var orders = orderLogic.Read(new OrderBindingModel { FreeOrders = true });
            foreach (var implementer in implementers)
            {
                WorkerWorkAsync(implementer, orders);
            }
        }

        /// Иммитация работы исполнителя
        private async void WorkerWorkAsync(ImplementerViewModel implementer, List<OrderViewModel> orders)
        {


        private async void WorkerWorkAsync(ImplementerViewModel implementer, List<OrderViewModel> orders)
        {

            var runOrders = await Task.Run(() => orderLogic.Read(new OrderBindingModel { ImplementerId = implementer.Id }));

            foreach (var order in runOrders)
            {

                // делаем работу заново
                Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                mainLogic.FinishOrder(new ChangeStatusBindingModel { OrderId = order.Id });

                // отдыхаем

                Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                mainLogic.FinishOrder(new ChangeStatusBindingModel { OrderId = order.Id });

                Thread.Sleep(implementer.PauseTime);
            }
            await Task.Run(() =>
            {
                foreach (var order in orders)
                {

                    // пытаемся назначить заказ на исполнителя
                    try
                    {
                        mainLogic.TakeOrderInWork(new ChangeStatusBindingModel { OrderId = order.Id, ImplementerId = implementer.Id });

                        // делаем работу
                        Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                        mainLogic.FinishOrder(new ChangeStatusBindingModel { OrderId = order.Id });

                        // отдыхаем

                    try
                    {
                        mainLogic.TakeOrderInWork(new ChangeStatusBindingModel { OrderId = order.Id, ImplementerId = implementer.Id });
                        Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                        mainLogic.FinishOrder(new ChangeStatusBindingModel { OrderId = order.Id });

                        Thread.Sleep(implementer.PauseTime);
                    }
                    catch (Exception) { }
                }
            });
        }
    }
}