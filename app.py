from flask import Flask, render_template, request, redirect, url_for, flash
from flask_sqlalchemy import SQLAlchemy
import os
import threading
import argparse

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///data.db'
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
app.config['SECRET_KEY'] = os.environ.get('FLASK_SECRET', 'dev-secret')

db = SQLAlchemy(app)


class Item(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    student_name = db.Column(db.String(120), nullable=False)
    student_id = db.Column(db.String(60), nullable=False)
    email = db.Column(db.String(120), nullable=True)

    def __repr__(self):
        return f"<Item {self.id} {self.student_name} {self.student_id}>"


with app.app_context():
    db.create_all()


@app.route('/')
def index():
    items = Item.query.order_by(Item.id.desc()).all()
    return render_template('index.html', items=items)


@app.route('/create', methods=['GET', 'POST'])
def create():
    if request.method == 'POST':
        student_name = request.form.get('student_name', '').strip()
        student_id = request.form.get('student_id', '').strip()
        email = request.form.get('email', '').strip()
        if not student_name or not student_id:
            flash('Student name and ID are required', 'danger')
            return redirect(url_for('create'))
        item = Item(student_name=student_name, student_id=student_id, email=email)
        db.session.add(item)
        db.session.commit()
        flash('Record created', 'success')
        return redirect(url_for('index'))
    return render_template('form.html', action='Create', item=None)


@app.route('/edit/<int:item_id>', methods=['GET', 'POST'])
def edit(item_id):
    item = Item.query.get_or_404(item_id)
    if request.method == 'POST':
        student_name = request.form.get('student_name', '').strip()
        student_id = request.form.get('student_id', '').strip()
        email = request.form.get('email', '').strip()
        if not student_name or not student_id:
            flash('Student name and ID are required', 'danger')
            return redirect(url_for('edit', item_id=item_id))
        item.student_name = student_name
        item.student_id = student_id
        item.email = email
        db.session.commit()
        flash('Record updated', 'success')
        return redirect(url_for('index'))
    return render_template('form.html', action='Edit', item=item)


@app.route('/delete/<int:item_id>', methods=['POST'])
def delete(item_id):
    item = Item.query.get_or_404(item_id)
    db.session.delete(item)
    db.session.commit()
    flash('Item deleted', 'success')
    return redirect(url_for('index'))


def run_server(host='127.0.0.1', port=5000, debug=False):
    app.run(host=host, port=port, debug=debug, use_reloader=False)


def run_gui(host='127.0.0.1', port=5000):
    try:
        import webview
    except Exception as e:
        print('pywebview is required for GUI mode. Install with `pip install pywebview`')
        raise

    url = f'http://{host}:{port}'
    server_thread = threading.Thread(target=run_server, args=(host, port, False), daemon=True)
    server_thread.start()
    webview.create_window('Simple CRUD App', url)


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--gui', action='store_true', help='Open app in a native window')
    parser.add_argument('--host', default='127.0.0.1')
    parser.add_argument('--port', default=5000, type=int)
    args = parser.parse_args()

    if args.gui:
        run_gui(host=args.host, port=args.port)
    else:
        run_server(host=args.host, port=args.port, debug=True)
