from app import app, db, Item


def run():
    with app.app_context():
        db.drop_all()
        db.create_all()
        client = app.test_client()

        # Create
        resp = client.post('/create', data={'student_name': 'Test Student', 'student_id': 'S123', 'email': 't@example.com'}, follow_redirects=True)
        assert b'Test Student' in resp.data
        assert b'S123' in resp.data

        item = Item.query.first()
        assert item is not None

        # Update

        resp = client.post(f'/edit/{item.id}', data={'student_name': 'Updated Student', 'student_id': 'S999', 'email': 'u@example.com'}, follow_redirects=True)
        assert b'Updated Student' in resp.data
        assert b'S999' in resp.data

        # Delete
        resp = client.post(f'/delete/{item.id}', follow_redirects=True)
        assert b'Updated Student' not in resp.data

    print('All tests passed')


if __name__ == '__main__':
    run()
