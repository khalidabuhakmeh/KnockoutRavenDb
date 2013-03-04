ko.bindingHandlers.executeOnEnter = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var allBindings = allBindingsAccessor();
        $(element).keypress(function (event) {
            var keyCode = (event.which ? event.which : event.keyCode);
            if (keyCode === 13) {
                allBindings.executeOnEnter.call(viewModel);
                return false;
            }
            return true;
        });
    }
};

function AppViewModel(seed) {
    var self = this;
    var data = seed || { items: [], messages: [], create: '/todos/create', update: '/todos/update' };

    self.messages = ko.observableArray();
    self.items = ko.observableArray();
    self.create = seed.create;
    self.update = seed.update;
    self.newTodo = ko.observable();

    self.noItems = ko.computed(function () {
        return self.items().length == 0;
    });

    self.complete = function (item) {
        self.items.remove(item);
        $.post(self.update, { Id: item.id, Text: item.text, IsCompleted: true }, function (result) {
            if (result.ok) {
                self.flash('successfully completed task');
            } else {
                self.flash('could not update task due to server error');
                self.items.push(item);
            }
        });
    };

    self.add = function () {
        var item = new TaskViewModel({ Text: self.newTodo() }, self.complete);
        self.items.push(item);

        $.post(self.create, { text: item.text }, function (result) {
            if (result.ok) {
                item.id = result.todo.Id;
                self.newTodo(null);
                self.flash('successfully added task');
            } else {
                self.flash('could not add task due to server error');
                self.remove(item);
            }
        });
    };

    self.flash = function (text) {
        var message = { text: text };
        self.messages.push(message);

        setTimeout(function () {
            self.messages.remove(message);
        }, 3000);
    };

    /* Seed Information */
    for (var i in data.items) {
        self.items.push(new TaskViewModel(data.items[i], self.complete));
    }

    for (var m in data.messages) {
        self.flash(data.messages[m]);
    }

    return self;
}

function TaskViewModel(seed, callback) {
    var self = this;
    seed = seed || { id : '', Text : '', IsCompleted : '' };

    self.id = seed.Id;
    self.text = seed.Text;
    self.isCompleted = seed.IsCompleted;

    self.complete = function () {
        if (callback)
            callback.call(self, self);
    };
}