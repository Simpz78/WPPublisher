using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPPublisher.Controller;
using Xunit;

namespace WPPublisher.Tests.UnitTests
{
    /// <summary>
    /// Classe di supporto alla classe di test per inizializzare ciò che serve alla classe stessa
    /// </summary>
    public class ActionsStartupTest : IDisposable
    {
        public Actions Actions;

        /// <summary>
        /// Costruttore e metodo di setup, in questa classe potremmo anche mettere la creazione o il popolamento
        /// del database oppure le istanze che servono a tutta la classe. Questi metodi vegono eseguiti UNA volta sola
        /// per tutti i test della classe e non per ogni test.
        /// </summary>
        public ActionsStartupTest()
        {
            Actions = new();
        }

        /// <summary>
        /// Rilascio le risorse
        /// </summary>
        public void Dispose()
        {
            Actions = null;
        }
    }

    public class ActionsTest : IClassFixture<ActionsStartupTest>
    {
        private ActionsStartupTest startup;

        public ActionsTest(ActionsStartupTest startup) : base()
        {
            this.startup = startup;
        }
    
    }
}
