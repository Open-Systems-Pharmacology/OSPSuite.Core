using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.UI.DTO.Commands;

namespace OSPSuite.UI
{
    public abstract class concern_for_history_item_dto_list : ContextSpecification<HistoryItemDTOList>
    {
        protected IList<IHistoryItemDTO> _underlyingList;

        protected override void Context()
        {
            _underlyingList = new List<IHistoryItemDTO>();
            sut = new HistoryItemDTOList(_underlyingList);
        }
    }

    public class When_retrieving_an_item_by_id : concern_for_history_item_dto_list
    {
        private HistoryItemDTO _historyItemDTO1;
        private HistoryItemDTO _historyItemDTO2;
        private HistoryItemDTO _historyItemDTO3;
        private HistoryItemDTO _historyItemDTO4;

        protected override void Context()
        {
            base.Context();

            _historyItemDTO1 = new HistoryItemDTO(A.Fake<ICommand>()){Id="toto"};
            _historyItemDTO2 = new HistoryItemDTO(A.Fake<ICommand>()) { Id = "tata" };
            _historyItemDTO3 = new HistoryItemDTO(A.Fake<ICommand>()) { Id = "titi" };
            _historyItemDTO4 = new HistoryItemDTO(A.Fake<ICommand>()) { Id = "tutu" };

            _historyItemDTO2.AddSubHistory(_historyItemDTO3);
            _historyItemDTO3.AddSubHistory(_historyItemDTO4);
            _underlyingList.Add(_historyItemDTO1);
            _underlyingList.Add(_historyItemDTO2);
        }
        [Observation]
        public void should_return_the_first_item_found_with_the_id()
        {
            sut.ItemById(_historyItemDTO1.Id).ShouldBeEqualTo(_historyItemDTO1);
            sut.ItemById(_historyItemDTO2.Id).ShouldBeEqualTo(_historyItemDTO2);
            sut.ItemById(_historyItemDTO3.Id).ShouldBeEqualTo(_historyItemDTO3);
            sut.ItemById(_historyItemDTO4.Id).ShouldBeEqualTo(_historyItemDTO4);
        }
    }
}	