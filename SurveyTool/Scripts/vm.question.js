var QuestionModel = function() {
    var self = this;

    self.id = ko.observable(0);
    self.title = ko.observable().extend({ required: true });
    self.type = ko.observable().extend({ required: true });
    self.body = ko.observable().extend({ required: true });
    self.isActive = ko.observable(true);
    self.required = ko.observable().extend({ required:false });

    //self.activeText = ko.computed(function() {
    //    return self.isActive() ? "true" : "false";
    //}, self);

    //self.activeText = ko.computed(function () {
    //    return self.required() ? "true" : "false";
    //}, self);

    //self.activeText = ko.computed(function () {
    //    if (self.type() === "Yes/No") {
    //        return "Yêu";
    //    }
    //    return "abc";
    //}, self);

    self.isValid = function() {
        return self.title.isValid() && self.type.isValid() && self.body.isValid();
    };

    self.enable = function() {
        self.isActive(true);
    };

    self.typeText = ko.computed(function () {
        return self.type() === 'Email' ? false : true;
    });

    self.enableRequired = function () {
        self.required(true);
    };

    self.disable = function() {
        self.isActive(false);
    };

    self.disableRequired = function () {
        self.required(false);
    };
};