var SurveyListModel = function() {
    var self = this;

    self.destroy = function (item, event) {
        var $target = $(event.target);

        if (confirm('Bạn có chắc muốn xóa?')) {
            $.post($target.attr('href'), function() {
                $target.parents('tr').remove();
            });
        }

        return false;
    };
};